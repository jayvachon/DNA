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
				if (currentMatch != null)
					OnArriveAtDestination (CurrentPoint);
				return;
			}

			if (p != null) {

				// If a task is being performed, stop it
				InterruptTask ();

				// If the selected object is a GridPoint with a road, move to it
				if (p.HasRoad) {
					CurrentPoint = null;
					positioner.Destination = p;
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
					OnInitPerformableTasks (performableTasks);
				}
				return performableTasks;
			}
		}

		GridPoint currentPoint;
		GridPoint CurrentPoint {
			get { return currentPoint; }
			set {

				// Early exit if being set to the same value
				if (currentPoint == value)
					return;

				// If the point was previously set, remove any listeners
				if (currentPoint != null) {
					currentPoint.RemoveVisitor (this);
					currentPoint.OnSetObject -= OnChangeCurrentPointObject;
				}
				
				currentPoint = value;

				// Add listeners if not being set to null
				if (currentPoint != null) {
					currentPoint.RegisterVisitor (this);
					currentPoint.OnSetObject += OnChangeCurrentPointObject;
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

		public bool Idle {
			get { return currentMatch == null && !positioner.Moving; }
		}

		GridPoint lastMatchedPoint = null;
		MatchResult currentMatch;
		Connection currentRoadConstruction;
		Positioner positioner;

		static float startRotation = 0f;
		static float StartRotation {
			get {
				float r = startRotation;
				startRotation += 30;
				if (startRotation > 360)
					startRotation -= 360;
				return r;
			}
		}

		public void SetStartPoint (GridPoint point) {
			positioner = new Positioner (MyTransform, point, StartRotation);
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
				if (TryConstructRoad ())
					return;
			}

			// Check if the point has any tasks to perform
			// If so, start the task. If the task needs a pair, listen for when the task completes
			// Failing that, check if the point is on a road under construction
			if (!TryStartMatch ()) {
				TryConstructRoad ();
			}
		}

		void OnChangeCurrentPointObject (IPathElementObject obj) {
			
			// Need to wait a frame so that construction site can have its cost set
			Coroutine.WaitForFixedUpdate (() => TryStartMatch ());
		}

		bool TryStartMatch () {
			MatchResult match = PointTaskMatch (CurrentPoint);
			if (match != null) {
				currentMatch = match;
				if (match.PairType != null) {
					BeginEncircling ();
					currentMatch.Match.onComplete += OnCompleteTask;
				}
				match.Start (true);
				return true;
			}
			return false;
		}

		bool TryConstructRoad () {
			currentRoadConstruction = CurrentPoint.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
			if (currentRoadConstruction != null) {
				MatchResult match = TaskMatcher.GetPerformable (this, currentRoadConstruction.Object as ITaskAcceptor);
				if (match != null) {
					currentMatch = match;
					currentMatch.Match.onComplete += OnCompleteRoad;
					match.Start ();
					return true;
				}
			} else {
				RoadConstructionPoint = null;
			}
			return false;
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

			t.onComplete -= OnCompleteTask;
			if (TryStartMatch ()) {
				return;
			}

			// Check if the last matched point can pair with the task
			if (lastMatchedPoint != null && TaskMatcher.GetPair (t, lastMatchedPoint) != null) {
				positioner.Destination = lastMatchedPoint;
			} else {
				
				// If not, find the closest one that can
				positioner.Destination = Pathfinder.FindNearestPoint (
					positioner.Destination,
					(GridPoint p) => { return TaskMatcher.GetPair (t, p) != null; }
				);
			}

			currentMatch = null;
			lastMatchedPoint = CurrentPoint;
		}

		void OnCompleteRoad (PerformerTask t) {
			RoadConstructionPoint = Array.Find (currentRoadConstruction.Points, x => x != RoadConstructionPoint);
			t.onComplete -= OnCompleteRoad;
		}

		void InterruptTask () {
			if (currentMatch != null) {
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Match.Stop ();
				currentMatch = null;
			}
		}

		// move this to its own class
		#region encircling

		void BeginEncircling () {
			int performCount = currentMatch.GetPerformCount ();
			float time = currentMatch.Match.Settings.Duration * performCount;
			positioner.RotateAroundPoint (time);
		}

		#endregion

		protected virtual void OnInitPerformableTasks (PerformableTasks p) {}

		#region IPathElementVisitor implementation
		Fermat surroundPositions = new Fermat (new Fermat.Settings (0.75f, 100, 0, Vector3.zero));

		int visitorIndex = 0;
		public int VisitorIndex {
			get { return visitorIndex; }
			set {
				visitorIndex = value;
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