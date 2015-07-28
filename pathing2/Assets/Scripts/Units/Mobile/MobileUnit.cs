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

		//public virtual bool OnBindActionable (IActionAcceptor acceptor) {
		public virtual bool OnBindActionable (PathPoint point) {
			IActionAcceptor acceptor = point.StaticUnit as IActionAcceptor;
			if (BoundAcceptor == acceptor) return false; 
			BoundAcceptor = acceptor;
			return StartActions (point);
		}

		public bool StartActions (PathPoint point) {
			#if DEBUG_MSG
			Debug.Log ("=================");
			#endif
			PathPoint otherPoint = Path.Points.Points.Find (x => x != point);
			AcceptableActions acceptorActions = BoundAcceptor.AcceptableActions;
			List<string> otherPointActions = new List<string> (
				otherPoint.StaticUnit.AcceptableActions.ActiveActions.Keys);

			List<string> matching = PerformableActions.GetBoundActions (
				new List<string> (acceptorActions.ActiveActions.Keys));
			
			// Do performer and acceptor have a matching action?
			// Performer & acceptor do not have matching action
			if (matching.Count == 0) {
				#if DEBUG_MSG
				Debug.Log ("no matching actions");
				#endif
				MoveToOtherPointWithAction (otherPointActions, otherPoint);
				return false;
			}

			string matchingId = matching[0];
			AcceptorAction matchingAction = acceptorActions[matchingId];
			bool matchingActionEnabled = matchingAction.Enabled && PerformableActions[matchingId].Enabled;
			PathPoint nearestPair = Pathfinder.Instance.FindNearestWithAction (
				point.Position, matchingAction.EnabledState.RequiredPair);
			
			#if DEBUG_MSG
			Debug.Log ("matching actions");
			#endif
			// Performer & acceptor have matching action
			// Does the matching action require a pair?
			matchingAction.Bind (Inventory);
			if (matchingAction.EnabledState.RequiresPair) {
				#if DEBUG_MSG
				Debug.Log ("requires pair");
				#endif
				// Does the other point on the path have the required pair?
				bool otherPointHasPair = matchingAction.EnabledState.AttemptPair (otherPoint.StaticUnit as IActionAcceptor);
				if (otherPointHasPair) {
					#if DEBUG_MSG
					Debug.Log ("pair is on path");
					#endif
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
					#if DEBUG_MSG
					Debug.Log ("pair not on path");
					#endif
					// Does a pair exist in the world?
					
					nearestPair = Pathfinder.Instance.FindNearestWithAction (
						point.Position, matchingAction.EnabledState.RequiredPair);

					if (nearestPair != null) {
						#if DEBUG_MSG
						Debug.Log ("nearest pair is " + nearestPair.StaticUnit);
						#endif
						return MoveToPointWithAction (nearestPair, otherPoint);
					} else {
						#if DEBUG_MSG
						Debug.Log ("no pair exists in world");
						#endif
						MoveToOtherPointWithAction (otherPointActions, otherPoint);
						return false;
					}
				}

			} else {
				
				// Is the action enabled?
				if (matchingActionEnabled) {

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
				#if DEBUG_MSG
				Debug.Log ("no pair exists - stop moving");
				#endif
				StopMoving (otherPoint);
				return false;
			} else {
				#if DEBUG_MSG
				Debug.Log ("replace point");
				#endif
				Destination = nearestPair;
				OnEndActions ();
				return false;
			}
		}

		void MoveToOtherPointWithAction (List<string> otherPointActions, PathPoint otherPoint) {

			List<string> otherMatching = PerformableActions.GetBoundActions (otherPointActions);
				
			// Does the other point in the path have a matching action?
			if (otherMatching.Count == 0) {
				#if DEBUG_MSG
				Debug.Log ("other point has no actions");
				#endif
				// Stop moving
				StopMoving (otherPoint);
			} else {
				#if DEBUG_MSG
				Debug.Log ("move to other point");
				#endif
				// Move to the next point
				OnEndActions ();
			}
		}

		void PerformBoundAction (string id, PathPoint point) {
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
			StartCoroutine (CoWaitForCompleteCircle ());
		}

		IEnumerator CoWaitForCompleteCircle () {
			while (MobileTransform.Working) {
				yield return null;
			}
			MoveToDestination ();
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
			}
		}

		void MoveToDestination () {
			PathPoint a = ((StaticUnit)BoundAcceptor).PathPoint;
			if (a == Destination && Path.Points.Count < 2)
				return;
			if (Path.Points.Points.Contains (Destination)) {
				PerformableActions["MoveOnPath"].Start ();
			} else {
				Path.Points.Clear ();
				Path.StopMoving ();
				Path.Points.Add (a);
				Path.Points.Add (Destination);	
				PerformableActions["MoveOnPath"].Start ();
			}
		}
	}
}