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
			//ActionHandler.instance.Bind (this);
			return StartActions (point);
		}

		public bool StartActions (PathPoint point) {
			
			/*PathPoint otherPoint = Path.Points.Points.Find (x => x != point);

			// Find paired actions on the path
			List<string> paired = new List<string> ();
			if (otherPoint != null) {
				paired = PerformableActions.GetPairedActionsBetweenAcceptors (
					BoundAcceptor, otherPoint.StaticUnit as IActionAcceptor);
			}*/

			/*List<string> paired = GetPairedActionsOnPath ();
			
			List<string> bound;
			if (paired.Count > 0) {

				// If there are paired actions, match them with the enabled actions on this unit
				bound = PerformableActions.GetBoundActions (paired);
			} else {

				// If there aren't any paired actions, find one that can be performed without a pair
				bound = PerformableActions.GetBoundActions (
					new List<string> (BoundAcceptor.AcceptableActions.EnabledActions.Keys));
				Path.Points.Remove (otherPoint);

				if (bound.Count > 0) {
					string toPerform = bound[0];
					while (PerformableActions[toPerform].EnabledState.RequiresPair && bound.Count > 0 && Path.Points.Count < 2) {
						
						PathPoint nearest = Pathfinder.Instance.FindNearestWithAction (
							point.Position, PerformableActions[toPerform].EnabledState.RequiredPair);

						if (nearest != null) {
							PerformableActions.Stop ("MoveOnPath");
							Path.Points.Add (nearest);
						}

						bound.Remove (toPerform);
						if (bound.Count > 0) {
							toPerform = bound[0];
						}
					}
				}
			}*/

			PathPoint otherPoint = Path.Points.Points.Find (x => x != point);

			bool pairedOnPath;
			List<string> actions = GetAcceptedActionsAtPoint (out pairedOnPath);
			
			if (!pairedOnPath) {

				if (actions.Count == 0) {

					Debug.Log ("heard");
				} else {

					string currentAction = actions[0];

					while (!CreatePathToPair (currentAction) && actions.Count > 0) {
						actions.Remove (currentAction);
						if (actions.Count > 0) 
							currentAction = actions[0];
					}
				}
			}

			/*if (!pairedOnPath && actions.Count > 0) {

				Path.Points.Remove (otherPoint);
				string toPerform = actions[0];
				
				while (PerformableActions[toPerform].EnabledState.RequiresPair && Path.Points.Count < 2 && actions.Count > 0) {
					
					PathPoint nearest = Pathfinder.Instance.FindNearestWithAction (
						point.Position, PerformableActions[toPerform].EnabledState.RequiredPair);

					if (nearest != null) {
						PerformableActions.Stop ("MoveOnPath");
						Path.Points.Add (nearest);
					} else {
						actions.Remove (toPerform);
						if (actions.Count > 0) toPerform = actions[0];
					}
				}
			}*/
			
			if (actions.Count > 0) {
				PerformableActions.Start (actions[0]);
				StartCoroutine (WaitForActions (() => StartActions (point)));
				return true;
			} 
			
			OnEndActions ();
			return false;
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