#undef DEBUG_MSG
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;
using GameInput;
using GameEvents;

namespace Units {

	public class MobileUnit : Unit, IActionPerformer, IBinder {

		MobileUnitTransform mobileTransform;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = UnitTransform as MobileUnitTransform;
				}
				return mobileTransform;
			}
		}

		MobileUnitClickable mobileClickable;
		public MobileUnitClickable MobileClickable {
			get {
				if (mobileClickable == null) {
					mobileClickable = UnitClickable as MobileUnitClickable;
				}
				return mobileClickable;
			}
		}

		Path path = null;
		public Path Path {
			get { 
				if (path == null) {
					IPathable pathable = MobileTransform as IPathable; 
					path = pathable.Path;
				}
				return path;
			}
		}

		MoveOnPath moveOnPath;
		PerformableActions performableActions = null;
		public PerformableActions PerformableActions { 
			get {
				if (performableActions == null) {
					performableActions = new PerformableActions (this, false);
					moveOnPath = new MoveOnPath (this);
					performableActions.Add (moveOnPath);
				}
				return performableActions;
			}
		}

		public IActionAcceptor BoundAcceptor { get; protected set; } //TODO: should be private set
		PathPoint CurrentPoint { get { return ((StaticUnit)BoundAcceptor).PathPoint; } }
		PathPoint Destination { get; set; }

		bool moveOnRelease = true;
		bool interrupt = false;

		public void Init (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
		}

		public override void OnSelect () {
			base.OnSelect ();
			Events.instance.AddListener<ClickEvent> (OnClickEvent);
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			Events.instance.RemoveListener<ClickEvent> (OnClickEvent);
		}

		protected virtual void OnBind () {}
		protected virtual void OnUnbind () {}

		public virtual bool OnBindActionable (PathPoint point) {
			IActionAcceptor acceptor = point.StaticUnit as IActionAcceptor;
			if (BoundAcceptor == acceptor) return false; 
			BoundAcceptor = acceptor;
			return StartActions (point);
		}

		// TODO: This is a monster that needs some seriouz cleanup, but it does work
		public bool StartActions (PathPoint point) {
			
			Log ("=================");
			
			PathPoint otherPoint = Path.Points.Points.Find (x => x != point);
			AcceptableActions acceptorActions = BoundAcceptor.AcceptableActions;
			List<string> otherPointActions = (otherPoint == null)
				? new List<string> (0) 
				: new List<string> (otherPoint.StaticUnit.AcceptableActions.ActiveActions.Keys);

			List<string> matching = PerformableActions.GetBoundActions (
				new List<string> (acceptorActions.ActiveActions.Keys));
			
			// Do performer and acceptor have a matching action?
			// Performer & acceptor do not have matching action
			if (matching.Count == 0) {
				Log ("no matching actions");
				MoveToOtherPointWithAction (otherPointActions, otherPoint);
				return false;
			}

			return PerformMatchingAction (matching, acceptorActions, point, otherPoint, otherPointActions);
		}

		bool PerformMatchingAction (List<string> matching, AcceptableActions acceptorActions, PathPoint point, PathPoint otherPoint, List<string> otherPointActions) {

			string matchingId = matching[0];
			AcceptorAction matchingAction = acceptorActions[matchingId];
			bool matchingActionEnabled = matchingAction.Enabled && PerformableActions[matchingId].Enabled;
			PathPoint nearestPair = Pathfinder.Instance.FindNearestWithAction (
				point.Position, matchingAction.EnabledState.RequiredPair);
			
			Log ("matching action");
			// Performer & acceptor have matching action
			// Does the matching action require a pair?
			matchingAction.Bind (Inventory);
			if (matchingAction.EnabledState.RequiresPair) {
				Log ("requires pair");
				// Does the other point on the path have the required pair?
				bool otherPointHasPair = matchingAction.EnabledState.AttemptPair (otherPoint.StaticUnit as IActionAcceptor);
				if (otherPointHasPair) {
					Log ("pair is on path");
					// Is the action enabled?
					if (matchingActionEnabled) {

						// Perform the action
						PerformBoundAction (matchingId, point);
						return true;
					} else {

						// Move to the next point
						OnEndActions ();
						return false;
					}
				} else {
					Log ("pair not on path");

					matching.Remove (matchingId);
					if (matching.Count > 0) {
						Log ("looking at other matching actions");
						return PerformMatchingAction (matching, acceptorActions, point, otherPoint, otherPointActions);
					}

					// Does a pair exist in the world?
					Log ("looking at other points in the world");
					nearestPair = Pathfinder.Instance.FindNearestWithAction (
						point.Position, matchingAction.EnabledState.RequiredPair);

					if (nearestPair != null) {
						Log ("nearest pair is " + nearestPair.StaticUnit);
						return MoveToPointWithAction (nearestPair, otherPoint);
					} else {
						Log ("no pair exists in world");
						MoveToOtherPointWithAction (otherPointActions, otherPoint);
						return false;
					}
				}

			} else {
				
				// Is the action enabled?
				if (matchingActionEnabled) {

					// Remove the other path point if this action does not require a pair, or if
					// the other point on the path is not the required pair
					if (matchingAction.EnabledState.RequiredPair == ""
						|| !otherPointActions.Contains (matchingAction.EnabledState.RequiredPair)) {
						Path.Points.Remove (otherPoint);
					}

					// Perform the action
					PerformBoundAction (matchingId, point);
					return true;
				} else {
					PathPoint otherMatching = Pathfinder.Instance.FindNearestWithAction (point.Position, matchingId);
					return MoveToPointWithAction (otherMatching, otherPoint);
				}
			}
		}

		void StopMoving (PathPoint removePoint) {
			Path.Points.Remove (removePoint);
			MobileTransform.StopMovingOnPath ();
		}

		bool MoveToPointWithAction (PathPoint nearestPair, PathPoint otherPoint) {

			if (nearestPair == null) {
				// Stop moving
				Log ("no pair exists - stop moving");
				StopMoving (otherPoint);
				return false;
			} else {
				Log ("replace point");
				Destination = nearestPair;
				OnEndActions ();
				return false;
			}
		}

		void MoveToOtherPointWithAction (List<string> otherPointActions, PathPoint otherPoint) {

			List<string> otherMatching = PerformableActions.GetBoundActions (otherPointActions);
				
			// Does the other point in the path have a matching action?
			if (otherMatching.Count == 0) {
				// Stop moving
				Log ("other point has no actions");
				StopMoving (otherPoint);
			} else {
				// Move to the next point
				Log ("move to other point");
				OnEndActions ();
			}
		}

		void PerformBoundAction (string id, PathPoint point) {
			if (interrupt) return;
			PerformableActions.Start (id);
			StartCoroutine (WaitForActions (() => StartActions (point)));
		}

		List<string> GetAcceptedActionsAtPoint (out bool pairedOnPath) {
			List<string> paired = GetPairedActionsOnPath ();
			if (paired == null || paired.Count == 0) {
				pairedOnPath = false;
				return PerformableActions.GetBoundActions (
					new List<string> (BoundAcceptor.AcceptableActions.EnabledActions.Keys));
			} else {
				pairedOnPath = true;
				return PerformableActions.GetBoundActions (paired);
			}
		}

		List<string> GetPairedActionsOnPath () {

			PathPoint otherPoint = Path.Points.Points.Find (x => x != CurrentPoint); // Probably just the next point on the path ?

			return (otherPoint == null)
				? null
				: PerformableActions.GetPairedActionsBetweenAcceptors (
					BoundAcceptor, otherPoint.StaticUnit as IActionAcceptor);
		}

		IEnumerator WaitForActions (System.Action action) {
			while (PerformableActions.Performing) yield return null;
			action ();
		}

		public virtual void OnEndActions () {
			StartCoroutine (CoWaitForCompleteCircle (ActionOnEndCircling));
		}

		void ActionOnEndCircling () {
			// Check if any new actions have become enabled
			// If none were enabled, move to the next point
			if (!MobileTransform.ArriveAtPoint (CurrentPoint)) {
				MoveToDestination ();
			}
		}

		IEnumerator CoWaitForCompleteCircle (System.Action onEnd) {

			// Wait to finish circling
			while (MobileTransform.Working) {
				yield return null;
			}

			onEnd ();
		}

		void InterruptAction () {
			interrupt = true;
			PerformableActions.StopAll ();
			StartCoroutine (CoWaitForCompleteCircle (InterruptOnEndCircling));
		}

		void InterruptOnEndCircling () {
			MoveToDestination ();
			interrupt = false;
		}

		public void OnDragEnter () {
			moveOnRelease = true;
			MobileTransform.Path.DragFromPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}

		void OnClickEvent (ClickEvent e) {
			if (e.left) return;
			UnitClickable clickable = e.GetClickedOfType<UnitClickable> ();
			if (clickable == null) return;
			Destination = clickable.StaticUnit.PathPoint;
			if (!PerformableActions.Performing) {
				MoveToDestination ();
			} else {
				InterruptAction ();
			}
		}

		void MoveToDestination () {
			if (CurrentPoint == Destination && Path.Points.Count < 2)
				return;
			if (Path.Points.Points.Contains (Destination)) {
				PerformableActions["MoveOnPath"].Start ();
			} else {
				Path.Points.Clear ();
				Path.StopMoving ();
				Path.Points.Add (CurrentPoint);
				Path.Points.Add (Destination);	
				PerformableActions["MoveOnPath"].Start ();
			}
		}

		void Log (string message) {
			#if DEBUG_MSG
			Debug.Log (message);
			#endif
		}
	}
}