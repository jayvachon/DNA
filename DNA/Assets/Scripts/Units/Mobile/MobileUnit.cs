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
				if (currentMatch == null)
					OnArriveAtDestination (CurrentPoint);
				return;
			}

			if (p != null) {

				// If a task is being performed, stop it
				InterruptTask ();

				// If the selected object is a GridPoint with a road, move to it
				if (p.HasRoad) {
					CurrentPoint = null;
					SetDestination (p);
				} else {

					// If the selected object is a GridPoint with a road under construction, move to it
					Connection underConstruction = p.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
					if (underConstruction != null) {
						SetRoadConstructionPoint (underConstruction);
					}
				}
				return;
			}

			// If the selected object is a Connection, move to the terminus of the road construction plan
			Connection c = u.Element as Connection;
			if (c != null) {
				SetRoadConstructionPoint (c);
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
		protected GridPoint CurrentPoint {
			get { return currentPoint; }
			private set {

				// Early exit if being set to the same value
				if (currentPoint == value)
					return;

				// If the point was previously set, remove any listeners
				if (currentPoint != null) {
					currentPoint.RemoveVisitor (this);
					currentPoint.OnSetObject -= OnChangeCurrentPointObject;
					foreach (Connection c in currentPoint.Connections) {
						c.OnSetObject -= OnChangeConnectionObject;
					}
				}
				
				currentPoint = value;

				// Add listeners if not being set to null
				if (currentPoint != null) {
					currentPoint.RegisterVisitor (this);
					currentPoint.OnSetObject += OnChangeCurrentPointObject;
					foreach (Connection c in currentPoint.Connections) {
						c.OnSetObject += OnChangeConnectionObject;
					}
				}
			}
		}

		GridPoint roadConstructionPoint;
		GridPoint RoadConstructionPoint {
			get { return roadConstructionPoint; }
			set {
				roadConstructionPoint = value;
				SetDestination (roadConstructionPoint);
			}
		}

		public bool Idle {
			get { return currentMatch == null && !positioner.Moving; }
		}

		GridPoint lastMatchedPoint = null;
		MatchResult currentMatch;
		MatchResult previousMatch;
		MatchResult flowerMatch;
		Connection currentRoadConstruction;
		protected Positioner2 positioner;

		static float startRotation = 180f;
		static float StartRotation {
			get {
				float r = startRotation;
				startRotation += 30;
				if (startRotation > 360)
					startRotation -= 360;
				return r;
			}
		}

		public void SetStartPoint (GridPoint startPoint, bool setPosition=true) {
			positioner = new Positioner2 (MyTransform, startPoint);
			positioner.onArriveAtDestination += OnArriveAtDestination;
			positioner.onEnterPoint += OnEnterPoint;
			positioner.onExitPoint += OnExitPoint;
			if (setPosition)
				Position = startPoint.Position.GetPointAroundAxis (StartRotation, 1.25f);
			CurrentPoint = startPoint;
		}

		void SetDestination (GridPoint point) {
			if (point != null) {
				positioner.Destination = point;
			}
		}

		void SetRoadConstructionPoint (Connection connection) {
			RoadConstructionPoint = System.Array.Find (connection.Points, x => x.HasRoad);
		}

		/**
		 *	Assigning tasks:
		 *	  1. With unit selected, right click a point and see if it has tasks (OnOverrideSelect -> PointHasTaskMatch)
		 *	  2. When unit arrives at destination, start task and add callback if task needs a pair (OnArriveAtDestination -> PointHasTaskMatch -> OnCompleteTask)
		 *	  3. (if task needs pair) Find the pair on the path and move to it (OnCompleteTask)
		 */

		void OnArriveAtDestination (GridPoint point, bool lookForPair=true) {
			
			CurrentPoint = point;

			// If a road construction site was assigned, build the road
			if (point == RoadConstructionPoint) {
				if (TryConstructRoad ())
					return;
			}

			// Check if the point has any tasks to perform
			// If so, start the task. If the task needs a pair, listen for when the task completes
			// Failing that, check if the point is on a road under construction
			// Failing that, check if there's a disabled task at this point that can be paired with an enabled task on another point			
			if (!TryStartMatch ()) {
				if (!TryConstructRoad () && lookForPair) {
					TryMoveToPair ();
				}
			}
		}

		void OnEnterPoint (GridPoint point) {
			if (point.Unit is Flower) {
				flowerMatch = PointTaskMatch (point);
				if (flowerMatch != null)
					flowerMatch.Start (true);
			}
		}

		void OnExitPoint (GridPoint point) {
			if (flowerMatch != null) {
				flowerMatch.Stop ();
				flowerMatch = null;
			}
		}

		void OnChangeCurrentPointObject (IPathElementObject obj) {
			
			// Need to wait a frame so that construction site can have its cost set
			Coroutine.WaitForFixedUpdate (
				() => {
					if (currentMatch == null) {
						if (!TryStartMatch ()) {
							TryMoveToPair ();
						}
					}
				}
			);
		}

		void OnChangeConnectionObject (IPathElementObject obj) {

			// Need to wait a frame so that construction site can have its cost set
			Coroutine.WaitForFixedUpdate (
				() => {
					if (currentMatch == null)
						TryConstructRoad ();
				}
			);
		}

		bool TryStartMatch () {

			// Check if the previously performed task has a pair on this point
			// Failing that, check for other performable tasks on this point			
			MatchResult match = TaskMatcher.GetPerformable (previousMatch, this, CurrentPoint.Unit as ITaskAcceptor);
			if (match == null)
				match = PointTaskMatch (CurrentPoint);

			if (match != null && match.Start (true)) {
				currentMatch = match;
				BeginEncircling ();
				currentMatch.Match.onComplete += OnCompleteTask;
				// match.Start (true);
				return true;
			}
			return false;
		}

		bool TryConstructRoad () {
			currentRoadConstruction = CurrentPoint.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
			if (currentRoadConstruction != null) {
				MatchResult match = TaskMatcher.GetPerformable (this, currentRoadConstruction.Object as ITaskAcceptor);
				if (match != null && match.Start ()) {
					currentMatch = match;
					currentMatch.Match.onComplete += OnCompleteRoad;
					// match.Start ();
					return true;
				}
			} else {
				RoadConstructionPoint = null;
			}
			return false;
		}

		bool TryMoveToPair () {
			MatchResult match = TaskMatcher.GetPerformable (this, CurrentPoint.Unit as ITaskAcceptor, false);	
			if (match != null && !match.Match.Enabled) {
				GridPoint d = Pathfinder.FindNearestPoint (
					CurrentPoint,
					(GridPoint p) => { return TaskMatcher.GetPair (match.Match, p, false) != null; }
				);
				SetDestination (d);
				return true;
			} else {
				return false;
			}
		}

		MatchResult PointTaskMatch (GridPoint point) {

			// Check if the point has any tasks to perform
			return TaskMatcher.GetPerformable (this, point.Unit as ITaskAcceptor);
		}

		void OnCompleteTask (PerformerTask t) {

			t.onComplete -= OnCompleteTask;
			
			bool taskNeedsPair = t.Settings.Pair != null;

			if (taskNeedsPair) {

				// Check if the last matched point can pair with the task
				if (!t.Settings.AlwaysPairNearest && lastMatchedPoint != null && TaskMatcher.GetPair (t, lastMatchedPoint) != null) {
					SetDestination (lastMatchedPoint);
				} else {
					
					// If not, find the closest one that can
					GridPoint d = Pathfinder.FindNearestPoint (
						positioner.Destination,
						(GridPoint p) => { return TaskMatcher.GetPair (t, p) != null; }
					);

					SetDestination (d);
				}
			} else {

				// If the completed task doesn't require a pair, check if any other tasks can be performed on this point or connection
				if (!TryStartMatch ()) {
					if (TryConstructRoad ()) {
						return;
					}
				} else {
					return;
				}

				// If completed task was construction, look for other construction
				if (t is CollectItem<InventorySystem.LaborGroup>) {
					MoveToConstruction (false);
				}
			}

			previousMatch = currentMatch;
			currentMatch = null;
			lastMatchedPoint = CurrentPoint;
		}

		void OnCompleteRoad (PerformerTask t) {
			MoveToConstruction ();
			t.onComplete -= OnCompleteRoad;
			previousMatch = currentMatch;
			currentMatch = null;
		}

		void MoveToConstruction (bool preferRoad=true) {
			
			// Find the nearest construction site, checking roads first if preferRoad is true

			GridPoint d = null;
			if (preferRoad) {
				d = FindRoadUnderConstruction ();
				if (d == null) {
					d = FindPointUnderConstruction ();
				}
			} else {
				d = FindPointUnderConstruction ();
				if (d == null) {
					d = FindRoadUnderConstruction ();
				}
			}

			if (d != null)
				SetDestination (d);
		}

		GridPoint FindPointUnderConstruction () {
			return Pathfinder.FindNearestPoint (
				positioner.Destination,
				(GridPoint p) => { return p.State == DevelopmentState.UnderConstruction; }
			);
		}

		GridPoint FindRoadUnderConstruction () {
			return Pathfinder.FindNearestPoint (
				positioner.Destination,
				(GridPoint p) => { return p.HasRoadConstruction; }
			);
		}

		void InterruptTask () {
			if (currentMatch != null) {
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Stop ();
				currentMatch = null;
			}
		}

		void BeginEncircling () {
			int performCount = currentMatch.GetPerformCount ();
			float time = currentMatch.Match.Settings.Duration * performCount;
			positioner.RotateAroundPoint (time);
		}

		protected virtual void OnInitPerformableTasks (PerformableTasks p) {}

		protected override void OnDisable () {
			base.OnDisable ();
			positioner.onArriveAtDestination -= OnArriveAtDestination;
			positioner.onEnterPoint -= OnEnterPoint;
			positioner.onExitPoint -= OnExitPoint;
		}

		#region scaling select
		Vector3 startScale = Vector3.zero;
		public Vector3 Scale {
			get { return MyTransform.localScale; }
			set {
				if (startScale == Vector3.zero) {
					startScale = MyTransform.localScale;
				}
				MyTransform.localScale = value;
			}
		}
		
		public override void OnSelect () {
			base.OnSelect ();
			Scale = Scale * 1.05f; // this ensures that the selected unit shows up in front of any others it's overlapping
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			Scale = startScale;
		}
		#endregion

		#region IPathElementVisitor implementation
		int visitorIndex = 0;
		public int VisitorIndex {
			get { return visitorIndex; }
			set { 
				visitorIndex = value;
				if (Idle)
					OnArriveAtDestination (CurrentPoint, false);
			}
		}
		#endregion

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this));
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
	}
}