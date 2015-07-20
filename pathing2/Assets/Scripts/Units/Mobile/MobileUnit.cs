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
			
			PathPoint otherPoint = Path.Points.Points.Find (x => x != point);
			AcceptableActions acceptorActions = BoundAcceptor.AcceptableActions;
			List<string> otherPointActions = new List<string> (
				otherPoint.StaticUnit.AcceptableActions.ActiveActions.Keys);

			List<string> matching = PerformableActions.GetBoundActions (
				new List<string> (acceptorActions.ActiveActions.Keys));
			
			// Do performer and acceptor have a matching action?
			// Performer & acceptor do not have matching action
			if (matching.Count == 0) {

				MoveToOtherPointWithAction (otherPointActions, otherPoint);
				return false;
			}

			string matchingId = matching[0];
			AcceptorAction matchingAction = acceptorActions[matchingId];
			bool matchingActionEnabled = matchingAction.Enabled && PerformableActions[matchingId].Enabled;
			PathPoint nearestPair = Pathfinder.Instance.FindNearestWithAction (
				point.Position, matchingAction.EnabledState.RequiredPair);
			
			Debug.Log (point.StaticUnit.Name + "  bound ---------- " + matchingId);
			// Performer & acceptor have matching action
			// Does the matching action require a pair?
			if (matchingAction.EnabledState.RequiresPair) {
				Debug.Log ("requires pair");
				// Does the other point on the path have the required pair?
				bool otherPointHasPair = matchingAction.EnabledState.AttemptPair (otherPoint.StaticUnit as IActionAcceptor);
				if (otherPointHasPair) {
					Debug.Log ("pair is on path");
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
					Debug.Log ("pair not on path");
					// Does a pair exist in the world?
					
					nearestPair = Pathfinder.Instance.FindNearestWithAction (
						point.Position, matchingAction.EnabledState.RequiredPair);

					if (nearestPair != null) {
						return MoveToPointWithAction (nearestPair, otherPoint);
					} else {
						Debug.Log ("look at other");
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
				StopMoving (otherPoint);
				return false;
			} else {
				Path.Points.Remove (otherPoint);
				PerformableActions.Stop ("MoveOnPath");
				Path.Points.Add (nearestPair);
				OnEndActions ();
				return false;
			}
		}

		void MoveToOtherPointWithAction (List<string> otherPointActions, PathPoint otherPoint) {

			List<string> otherMatching = PerformableActions.GetBoundActions (otherPointActions);
				
			// Does the other point in the path have a matching action?
			if (otherMatching.Count == 0) {

				// Stop moving
				StopMoving (otherPoint);
			} else {

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

		bool CreatePathToPair (string actionId) {
			
			PerformerAction action = PerformableActions[actionId];

			if (!action.EnabledState.RequiresPair) {
				PerformableActions.Stop ("MoveOnPath");
				Path.Points.Remove (Path.Points.Points.Find (x => x != CurrentPoint));
				return true;
			}

			PathPoint nearest = Pathfinder.Instance.FindNearestWithAction (
				CurrentPoint.Position, action.EnabledState.RequiredPair);

			if (nearest != null) {
				PerformableActions.Stop ("MoveOnPath");
				Path.Points.Remove (Path.Points.Points.Find (x => x != CurrentPoint));
				Path.Points.Add (nearest);
				Destination = nearest;
				return true;;
			}

			return false;
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