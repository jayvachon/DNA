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

	public class MobileUnit : Unit, ITaskPerformer, IPointerDownHandler, ISelectableOverrider {

		#region ISelectableOverrider implementation
		public UnityEngine.EventSystems.PointerEventData.InputButton OverrideButton {
			get { return UnityEngine.EventSystems.PointerEventData.InputButton.Right; }
		}

		public virtual void OnOverrideSelect (ISelectable overridenSelectable) {
			StaticUnit u = overridenSelectable as StaticUnit;

			GridPoint p = (GridPoint)u.Element;
			if (PointTaskMatch (p) != null)
				TaskPoint = p;
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

		Positioner positioner;

		public void SetStartPoint (GridPoint point) {
			positioner = new Positioner (MobileTransform.MyTransform, point);
			positioner.OnArriveAtDestination += OnArriveAtDestination;
		}

		/**
		 *	Assigning tasks:
		 *	  1. With unit selected, right click a point and see if it has tasks (OnOverrideSelect -> PointHasTaskMatch)
		 *	  2. When unit arrives at destination, start task and add callback if task needs a pair (OnArriveAtDestination -> PointHasTaskMatch -> OnCompleteTask)
		 *	  3. (if task needs pair) Find the pair on the path and move to it (OnCompleteTask)
		 */

		void OnArriveAtDestination (GridPoint point) {

			// Check if the point has any tasks to perform
			// If so, start the task. If the task needs a pair, listen for when the task completes
			MatchResult match = PointTaskMatch (point);
			if (match != null) {
				if (match.PairType != null)
					match.Match.onComplete += OnCompleteTask;
				match.Start (true);
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
			GridPoint p = DNA.Paths.Pathfinder.ConnectedPoints.FirstOrDefault (
				x => TaskMatcher.GetPair (match.Match, x) != null);

			return (p == null) ? null : match;
		}

		void OnCompleteTask (PerformerTask t) {

			// Check if the assigned TaskPoint can pair with the task
			if (TaskMatcher.GetPair (t, TaskPoint) != null) {
				positioner.Destination = TaskPoint;
			} else {

				// If not, find the closest one that can
				positioner.Destination = DNA.Paths.Pathfinder.FindNearestPoint (
					positioner.Destination,
					(GridPoint p) => { return TaskMatcher.GetPair (t, p) != null; }
				);
			}

			t.onComplete -= OnCompleteTask;
		}


		/*IEnumerator CoWaitForCompleteCircle (System.Action onEnd) {

			// Wait to finish circling
			while (MobileTransform.Working)
				yield return null;

			onEnd ();
		}*/

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
	}
}