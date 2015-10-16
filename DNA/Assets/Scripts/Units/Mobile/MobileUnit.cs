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
				positioner.Destination = p;
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

		Positioner positioner;

		public void SetStartPoint (GridPoint point) {
			positioner = new Positioner (MobileTransform.MyTransform, point);
			positioner.OnArriveAtDestination += OnArriveAtDestination;
		}

		void OnArriveAtDestination (GridPoint point) {
			MatchResult match = PointTaskMatch (point);
			if (match.PairType != null)
				match.Match.onComplete += OnCompleteTask;
			if (match != null)
				match.Start (true);
		}

		MatchResult PointTaskMatch (GridPoint point) {

			ITaskAcceptor acceptor = point.Unit as ITaskAcceptor;
			MatchResult match = TaskMatcher.GetPerformable (this, acceptor);

			if (match == null)
				return null;

			if (!match.NeedsPair)
				return match;

			GridPoint p = DNA.Paths.Pathfinder.ConnectedPoints.FirstOrDefault (
				x => x.Object != null 
				&& x.Object is Unit
				&& TaskMatcher.GetPair (match.Match, ((ITaskAcceptor)x.Unit)) != null);

			return (p == null) ? null : match;
		}

		void OnCompleteTask (PerformerTask t) {
			
			// TODO: this should find the closest acceptor
			GridPoint p = DNA.Paths.Pathfinder.ConnectedPoints.FirstOrDefault (
				x => x.Object != null 
				&& x.Object is Unit
				&& TaskMatcher.GetPair (t, ((ITaskAcceptor)x.Unit)) != null);

			positioner.Destination = p;

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