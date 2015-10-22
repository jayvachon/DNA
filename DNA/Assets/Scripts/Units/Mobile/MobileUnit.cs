#undef DEBUG_MSG
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class MobileUnit : Unit, ITaskPerformer, IPointerDownHandler, ISelectableOverrider, IPathElementVisitor {

		#region ISelectableOverrider implementation
		public UnityEngine.EventSystems.PointerEventData.InputButton OverrideButton {
			get { return UnityEngine.EventSystems.PointerEventData.InputButton.Right; }
		}

		public virtual void OnOverrideSelect (ISelectable overridenSelectable) {

			StaticUnit u = overridenSelectable as StaticUnit;

			// Only interested in StaticUnits
			if (u == null)
				return;

			GridPoint p = u.Element as GridPoint;

			// If the current point was selected and a task is not being performed, run OnArrive again to check for updates
			if (p == CurrentPoint) {
				if (currentTask != null)
					OnArriveAtDestination (CurrentPoint);
				return;
			}

			if (p != null) {

				// If a task is being performed, stop it
				InterruptTask ();

				// If the selected object is a GridPoint with a road, move to it
				if (p.HasRoad) {
					CurrentPoint = null;
					TaskPoint = p;
				} else {

					// If the selected object is a GridPoint with a road under construction, move to it
					Connection underConstruction = p.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
					if (underConstruction != null) {
						RoadConstructionPoint = ((ConstructionSite)underConstruction.Object).RoadPlan.Terminus;
					}
				}
				return;
			}

			// If the selected object is a Connection, move to the terminus of the road construction plan
			Connection c = u.Element as Connection;
			if (c != null) {
				RoadConstructionPoint = ((ConstructionSite)c.Object).RoadPlan.Terminus;
			}
		}
		#endregion

		MobileUnitTransform mobileTransform;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = UnitTransform as MobileUnitTransform;
				}
				return mobileTransform;
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

		GridPoint taskPoint;
		GridPoint TaskPoint {
			get { return taskPoint; }
			set {
				taskPoint = value;
				positioner.Destination = taskPoint;
			}
		}

		GridPoint currentPoint;
		GridPoint CurrentPoint {
			get { return currentPoint; }
			set {
				if (currentPoint == value)
					return;
				if (currentPoint != null)
					currentPoint.RemoveVisitor (this);
				currentPoint = value;
				if (currentPoint != null)
					currentPoint.RegisterVisitor (this);
				if (currentPoint != null && currentPoint == TaskPoint) {
					TaskPoint.OnSetObject += OnChangeTaskPointObject;
				} else if (TaskPoint != null) {
					TaskPoint.OnSetObject -= OnChangeTaskPointObject;
				}
			}
		}

		GridPoint roadConstructionPoint;
		GridPoint RoadConstructionPoint {
			get { return roadConstructionPoint; }
			set {
				roadConstructionPoint = value;
				positioner.Destination = roadConstructionPoint;
			}
		}

		PerformerTask currentTask;
		Connection currentRoadConstruction;
		Positioner positioner;

		public void SetStartPoint (GridPoint point) {
			positioner = new Positioner (MobileTransform.MyTransform, point);
			positioner.OnArriveAtDestination += OnArriveAtDestination;
			CurrentPoint = point;
		}

		/**
		 *	Assigning tasks:
		 *	  1. With unit selected, right click a point and see if it has tasks (OnOverrideSelect -> PointHasTaskMatch)
		 *	  2. When unit arrives at destination, start task and add callback if task needs a pair (OnArriveAtDestination -> PointHasTaskMatch -> OnCompleteTask)
		 *	  3. (if task needs pair) Find the pair on the path and move to it (OnCompleteTask)
		 */

		void OnArriveAtDestination (GridPoint point) {

			CurrentPoint = point;

			// If a road construction site was assigned, build the road
			if (point == RoadConstructionPoint) {
				TryConstructRoad ();
				return;
			}

			// Check if the point has any tasks to perform
			// If so, start the task. If the task needs a pair, listen for when the task completes
			// Failing that, check if the point is on a road under construction
			if (!TryStartMatch ()) {
				TryConstructRoad ();
			}
		}

		void OnChangeTaskPointObject (IPathElementObject obj) {
			TryStartMatch ();
		}

		bool TryStartMatch () {
			MatchResult match = PointTaskMatch (CurrentPoint);
			if (match != null) {
				currentTask = match.Match;
				if (match.PairType != null)
					currentTask.onComplete += OnCompleteTask;
				match.Start (true);
				return true;
			}
			return false;
		}

		void TryConstructRoad () {
			currentRoadConstruction = CurrentPoint.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
			if (currentRoadConstruction != null) {
				MatchResult match = TaskMatcher.GetPerformable (this, currentRoadConstruction.Object as ITaskAcceptor);
				if (match != null) {
					currentTask = match.Match;
					currentTask.onComplete += OnCompleteRoad;
					match.Start ();
				}
			}
		}

		MatchResult PointTaskMatch (GridPoint point) {

			// Check if the point has any tasks to perform
			ITaskAcceptor acceptor = point.Unit as ITaskAcceptor;
			MatchResult match = TaskMatcher.GetPerformable (this, acceptor);

			if (match == null)
				return null;

			if (!match.NeedsPair)
				return match;

			// If it does but needs a pair, check if a pair exists in the world
			GridPoint p = Pathfinder.ConnectedPoints.FirstOrDefault (
				x => TaskMatcher.GetPair (match.Match, x) != null);

			return (p == null) ? null : match;
		}

		void OnCompleteTask (PerformerTask t) {

			// Check if the assigned TaskPoint can pair with the task
			if (TaskMatcher.GetPair (t, TaskPoint) != null) {
				positioner.Destination = TaskPoint;
			} else {

				// If not, find the closest one that can
				positioner.Destination = Pathfinder.FindNearestPoint (
					positioner.Destination,
					(GridPoint p) => { return TaskMatcher.GetPair (t, p) != null; }
				);
			}

			t.onComplete -= OnCompleteTask;
			currentTask = null;
		}

		void OnCompleteRoad (PerformerTask t) {
			RoadConstructionPoint = Array.Find (currentRoadConstruction.Points, x => x != RoadConstructionPoint);
			t.onComplete -= OnCompleteRoad;
		}

		void InterruptTask () {
			if (currentTask != null) {
				currentTask.onComplete -= OnCompleteTask;
				currentTask.Stop ();
				currentTask = null;
			}
		}

		/*IEnumerator CoWaitForCompleteCircle (System.Action onEnd) {

			// Wait to finish circling
			while (MobileTransform.Working)
				yield return null;

			onEnd ();
		}*/

		IEnumerator CoMoveToYPosition (float endY) {
			
			float time = 0.15f;
			float eTime = 0f;
			float startY = Transform.localPosition.y;
		
			while (eTime < time) {
				eTime += Time.deltaTime;
				float progress = Mathf.SmoothStep (0, 1, eTime / time);
				Transform.SetLocalPositionY (Mathf.Lerp (startY, endY, progress));
				yield return null;
			}

			Transform.SetLocalPositionY (endY);
		}

		#region IPathElementVisitor implementation
		int visitorIndex = 0;
		public int VisitorIndex {
			get { return visitorIndex; }
			set {
				visitorIndex = value;
				if (visitorIndex > 0)
					StartCoroutine (CoMoveToYPosition (7.5f + 1.15f * visitorIndex));
				else
					StartCoroutine (CoMoveToYPosition (0f));
			}
		}
		#endregion

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
	}
}