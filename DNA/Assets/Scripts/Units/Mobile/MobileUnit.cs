#undef DEBUG_MSG
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;

namespace DNA.Units {

	public class MobileUnit : Unit, ITaskPerformer, IPointerDownHandler {

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

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
				}
				return performableTasks;
			}
		}

		PathPoint CurrentPoint { 
			get {
				PathPoint currPoint = Path.Positioner.CurrentPoint;
				if (currPoint == null) {
					return givingTree;
				}
				return currPoint;
			}
		}

		PathPoint Destination { get; set; }

		protected PathPoint givingTree;
		PathPoint currentPoint; // TODO: move to PathPoints

		public void Init (PathPoint givingTree) {
			this.givingTree = givingTree;
		}

		public bool OnArriveAtPoint (PathPoint point) {
			
			currentPoint = point;
			PathPoint otherPoint = Path.Points.Points.Find (x => x != point);
			ITaskAcceptor acceptor = point.StaticUnit as ITaskAcceptor;
			ITaskAcceptor acceptorPair = otherPoint.StaticUnit as ITaskAcceptor;

			MatchResult match = TaskMatcher.GetPerformable (this, acceptor, acceptorPair);
			if (match != null) {
				if (!match.NeedsPair) {
					/*if (match.PairType == null) {
						Path.Points.Remove (otherPoint);
					}*/
					match.Match.onComplete += OnCompleteTask;
					match.Start ();
					return true;
				} else {
					// TODO: use pathfinder to try to find a pair in the world
				}
			} 
			
			CheckOtherPointForMatches (acceptor, acceptorPair);

			return false;
		}

		void CheckOtherPointForMatches (ITaskAcceptor acceptor, ITaskAcceptor acceptorPair) {
			
			MatchResult match = TaskMatcher.GetPerformable (this, acceptorPair, acceptor);

			if (match == null) {
				StaticUnit su = (StaticUnit)acceptor;
				StopMoving (su.PathPoint);
			} else {
				OnEndTasks ();
			}
		}

		void OnCompleteTask (PerformerTask task) {
			task.onComplete -= OnCompleteTask;
			if (!OnArriveAtPoint (currentPoint)) {
				MobileTransform.OnCompleteTask ();
			}
		}

		void StopMoving (PathPoint removePoint) {
			Path.Points.Remove (removePoint);
			MobileTransform.StopMovingOnPath ();
		}

		public void OnEndTasks () {
			StartCoroutine (CoWaitForCompleteCircle (MoveToDestination));
		}

		IEnumerator CoWaitForCompleteCircle (System.Action onEnd) {

			// Wait to finish circling
			while (MobileTransform.Working)
				yield return null;

			onEnd ();
		}

		/*void OnClickEvent (ClickEvent e) {
			if (e.left) return;
			UnitClickable clickable = e.GetClickedOfType<UnitClickable> ();
			if (clickable == null) return;
			Destination = clickable.StaticUnit.PathPoint;

			MoveToDestination ();
			//if (!PerformableActions.Performing) {
				//MoveToDestination ();
			//} else {
				//InterruptAction ();
			//}
		}*/

		void MoveToDestination () {
			if (CurrentPoint == Destination && Path.Points.Count < 2)
				return;
			if (Path.Points.Points.Contains (Destination)) {
				Debug.Log ("start");
				Path.StartMoving ();
			} else {
				Debug.Log ("something else");
				Path.Points.Clear ();
				Path.StopMoving ();
				Path.Points.Add (CurrentPoint);
				Path.Points.Add (Destination);
				Path.StartMoving ();
			}
		}

		void Log (string message) {
			#if DEBUG_MSG
			Debug.Log (message);
			#endif
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
	}
}