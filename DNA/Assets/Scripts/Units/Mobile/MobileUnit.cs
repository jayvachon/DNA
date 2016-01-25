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

			AssignPoint (u.Element);
		}
		/*public virtual void OnOverrideSelect (ISelectable overridenSelectable) {

			StaticUnit u = overridenSelectable as StaticUnit;

			// Only interested in StaticUnits
			if (u == null)
				return;

			GridPoint p = u.Element as GridPoint;

			// If the current point was selected and a task is not being performed, run OnArrive again to check for updates
			if (p == CurrentPoint) {
				//if (currentMatch == null)
				//	OnArriveAtDestination (CurrentPoint);
				if (Idle) OnArriveAtDestination (CurrentPoint);
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
		}*/
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

		public bool Idle {
			get { return MyState == State.Idle; }
		}

		enum State {
			Idle,
			Moving,
			Waiting,
			Working
		}

		State state = State.Idle;
		State MyState {
			get { return state; }
			set { state = value; }
		}

		/*GridPoint currentPoint;
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
			get { return MyState == State.Idle; }
		}

		enum State {
			Idle,
			Moving,
			Waiting,
			Working
		}*/

		/*State state = State.Idle;
		State MyState {
			get { return state; }
			set {
				if (state == State.Waiting) {
					if (currentMatch != null) {
						currentMatch.Match.CancelWait ();
						currentMatch.Match.onEndWait -= OnEndWait;
					}
				}
				state = value; 
			}
		}

		GridPoint lastMatchedPoint = null;
		MatchResult currentMatch;
		MatchResult previousMatch;
		MatchResult flowerMatch;
		Connection currentRoadConstruction;*/
		protected Positioner2 positioner;
		PathElement assignedElement;
		MatchResult flowerMatch;

		TaskSelection taskSelector;
		TaskSelection TaskSelector {
			get {
				if (taskSelector == null) {
					taskSelector = new TaskSelection (this);
				}
				return taskSelector;
			}
		}

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
			/*CurrentPoint = startPoint;*/
		}

		void AssignPoint (PathElement elem) {

			switch (MyState) {
				case State.Idle: SetDestination (elem); break;
				case State.Moving: SetDestination (elem); break;
				case State.Waiting:
				case State.Working:
					if (elem != TaskSelector.CurrentElem) {
						TaskSelector.InterruptTask ();
						SetDestination (elem);
					}
					break;
			}
		}

		void SetDestination (PathElement elem) {

			assignedElement = elem;

			positioner.Destination = (elem is GridPoint)
				? (GridPoint)elem
				: System.Array.Find (((Connection)elem).Points, x => x.HasRoad);

			if (positioner.Moving)
				MyState = State.Moving;
			else
				OnArriveAtDestination ((GridPoint)elem);
		}

		void OnArriveAtDestination (GridPoint point) {
			
			ArriveAction action = TaskSelector.OnArrive (assignedElement);
			
			if (action.Destination != null) {
				SetDestination (action.Destination);
				return;
			}

			switch (action.StartResult) {
				case TaskStartResult.Success:
					OnBeginTask ();
					break;
				case TaskStartResult.Wait:
					MyState = State.Waiting;
					break;
				default:
					MyState = State.Idle;
					break;
			}
		}

		void OnCompleteTask () {
			MyState = State.Idle;
		}

		void OnBeginTask () {
			MyState = State.Working;
			positioner.RotateAroundPoint (TaskSelector.PerformTime);
		}

		void OnEnterPoint (GridPoint point) {
			if (point.Unit is Flower) {
				flowerMatch = TaskMatcher.GetPerformable (this, point.Unit as ITaskAcceptor);
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

		struct ArriveAction {

			public readonly PathElement Destination;
			public readonly TaskStartResult StartResult;

			public ArriveAction (PathElement destination, TaskStartResult startResult) {
				Destination = destination;
				StartResult = startResult;
			}
		}

		class TaskSelection {

			public float PerformTime {
				get { return currentMatch.Match.Settings.Duration * currentMatch.GetPerformCount (); }
			}

			public PathElement CurrentElem {
				get { return currentElem; }
			}

			MatchResult currentMatch;
			MatchResult previousMatch;
			PathElement currentElem;
			PathElement previousElem;
			MobileUnit unit;

			public TaskSelection (MobileUnit unit) {
				this.unit = unit;
			}

			public ArriveAction OnArrive (PathElement elem) {

				previousElem = currentElem;
				currentElem = elem;

				// Early out if the object associated with this PathElement does accept tasks
				if (!(elem.Object is ITaskAcceptor)) {
					return new ArriveAction (null, TaskStartResult.Null);
				}
				
				// First, try to perform a task on this PathElement
				// Failing that, try to perform a task on an associated Connection or GridPoint

				TaskStartResult result = TaskStartResult.Null;
				if (!TryStartMatch (elem, out result)) {
					
					GridPoint point = elem as GridPoint;
					if (point != null) {
						if (TryStartMatch (point.Connections.Find (x => x.State == DevelopmentState.UnderConstruction), out result)) {
							currentElem = point as PathElement;
						}
					} else {
						Connection connection = elem as Connection;
						if (TryStartMatch (System.Array.Find (connection.Points, x => 
							x.State == DevelopmentState.UnderConstruction 
							|| x.State == DevelopmentState.Developed 
							|| x.State == DevelopmentState.UnderRepair), 
							out result)) {
							currentElem = connection as PathElement;
						}
					}
				}

				// If no task was found, search for a pair

				PathElement destination = null;
				if (result == TaskStartResult.Null || result == TaskStartResult.Disabled) {
					TryGetPairDestination (elem, out destination);
				}

				return new ArriveAction (destination, result);
			}

			public void InterruptTask () {
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Match.CancelWait ();
				currentMatch.Stop ();
				currentMatch = null;
			}

			bool TryStartMatch (PathElement elem, out TaskStartResult startResult) {

				if (elem != null) {

					MatchResult result;
					if (TryGetPrevious (elem, out result)) {
						startResult = StartMatch (result);
						return true;
					} else if (TryGetFromPoint (elem, out result)) {
						startResult = StartMatch (result);
						return true;
					}
				}

				startResult = TaskStartResult.Null;
				return false;
			}

			TaskStartResult StartMatch (MatchResult match) {

				TaskStartResult result = match.Start (true);

				switch (result) {
					case TaskStartResult.Disabled:
					case TaskStartResult.Null: return result;
					case TaskStartResult.Success:
						match.Match.onComplete += OnCompleteTask;
						break;
					case TaskStartResult.Wait:
						match.Match.onEndWait += OnEndWait;
						break;
				}

				currentMatch = match;
				return result;
			}

			bool TryGetPrevious (PathElement elem, out MatchResult result) {

				result = (previousMatch == null)
					? null
					: TaskMatcher.GetPerformable (previousMatch, unit, elem.Object as ITaskAcceptor);

				return result != null;
			}

			bool TryGetFromPoint (PathElement elem, out MatchResult result) {
				result = TaskMatcher.GetPerformable (unit, elem.Object as ITaskAcceptor);
				return result != null;
			}

			bool TryGetPairDestination (PathElement elem, out PathElement destination) {
				MatchResult match = TaskMatcher.GetPerformable (unit, elem.Object as ITaskAcceptor, false);	
				if (match != null && !match.Match.Enabled) {
					return TryGetNearestElemWithTask (elem, match.Match, out destination);
				}
				destination = null;
				return false;
			}

			bool TryGetNearestElemWithTask (PathElement elem, PerformerTask task, out PathElement destination) {
				destination = Pathfinder.FindNearestPoint (
					(GridPoint)elem,
					(GridPoint p) => { return TaskMatcher.GetPair (task, p) != null; }
				);
				return destination != null;
			}

			bool TryFindNearestConstruction (GridPoint point, out PathElement destination) {
				destination = Pathfinder.FindNearestPoint (
					point,
					(GridPoint p) => { return p.HasRoadConstruction || p.State == DevelopmentState.UnderConstruction; }
				);
				return destination != null;
			}

			void OnCompleteTask (PerformerTask task) {

				task.onComplete -= OnCompleteTask;
				bool taskNeedsPair = task.Settings.Pair != null;

				if (taskNeedsPair) {

					// Find the task's pair and move to it.
					// Unless otherwise set, prioritize the previous pair.					

					if (!task.Settings.AlwaysPairNearest 
						&& previousElem != null 
						&& TaskMatcher.GetPair (task, previousElem) != null) {
						unit.SetDestination (previousElem);
					} else {
						PathElement destination;
						if (TryGetNearestElemWithTask (currentElem, task, out destination)) {
							unit.SetDestination (destination);
						} else {
							unit.OnCompleteTask ();
						}
					}

					previousMatch = currentMatch;
					
				} else {

					// Look for other tasks on the point
					// Finding none, look for other instances of the task just performed

					GridPoint point = currentElem as GridPoint;
					if (point != null) {
						unit.OnArriveAtDestination (point);
					} else {
						unit.OnCompleteTask ();
					}

					if (unit.Idle) {

						if (point == null)
							point = ((Connection)currentElem).Points[0];

						PathElement destination;
						if (TryFindNearestConstruction (point, out destination)) {
							unit.SetDestination (destination);
						}
					}
				}
			}

			void OnEndWait (PerformerTask task, bool performing) {
				task.onEndWait -= OnEndWait;
				if (performing) {
					task.onComplete += OnCompleteTask;
					unit.OnBeginTask ();
				} else {
					unit.OnCompleteTask ();
				}
			}
		}

		/*void SetDestination (GridPoint point) {
			if (point != null) {
				positioner.Destination = point;
				MyState = State.Moving;
			} else {
				MyState = State.Idle;
			}
		}

		void SetRoadConstructionPoint (Connection connection) {
			RoadConstructionPoint = System.Array.Find (connection.Points, x => x.HasRoad);
		}*/

		/**
		 *	Assigning tasks:
		 *	  1. With unit selected, right click a point and see if it has tasks (OnOverrideSelect -> PointHasTaskMatch)
		 *	  2. When unit arrives at destination, start task and add callback if task needs a pair (OnArriveAtDestination -> PointHasTaskMatch -> OnCompleteTask)
		 *	  3. (if task needs pair) Find the pair on the path and move to it (OnCompleteTask)
		 */

		/*void OnArriveAtDestination (GridPoint point, bool lookForPair=true) {
			
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
				if (!TryConstructRoad ()) {
					if (lookForPair && TryMoveToPair ()) {
						return;
					} else {
						MyState = State.Idle;
					}
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

			if (match == null) {
				match = PointTaskMatch (CurrentPoint);
				if (match == null) return false;
			}

			TaskStartResult result = match.Start (true);

			if (result == TaskStartResult.Disabled)
				return false;

			currentMatch = match;

			if (result == TaskStartResult.Success) {
				MyState = State.Working;
				BeginEncircling ();
				currentMatch.Match.onComplete += OnCompleteTask;
				return true;
			} else if (result == TaskStartResult.Wait) {
				MyState = State.Waiting;
				currentMatch.Match.onEndWait += OnEndWait;
				return true;
			}
			return false;
		}

		bool TryConstructRoad () {
			currentRoadConstruction = CurrentPoint.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
			if (currentRoadConstruction != null) {

				MatchResult match = TaskMatcher.GetPerformable (this, currentRoadConstruction.Object as ITaskAcceptor);

				if (match == null)
					return false;

				TaskStartResult result = match.Start ();

				if (result == TaskStartResult.Disabled)
					return false;

				currentMatch = match;

				if (result == TaskStartResult.Success) {
					MyState = State.Working;
					currentMatch.Match.onComplete += OnCompleteRoad;
					return true;
				} else if (result == TaskStartResult.Wait) {
					MyState = State.Waiting;
					currentMatch.Match.onEndWait += OnEndWait;
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
			MyState = State.Idle;

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
			MyState = State.Idle;
			MoveToConstruction ();
			t.onComplete -= OnCompleteRoad;
			previousMatch = currentMatch;
			currentMatch = null;
		}

		void OnEndWait (PerformerTask t, bool performing) {
			if (performing) {
				MyState = State.Working;
				BeginEncircling ();
				currentMatch.Match.onComplete += OnCompleteTask;
			} else {
				MyState = State.Idle;
			}
			t.onEndWait -= OnEndWait;
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
				MyState = State.Idle;
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Stop ();
				currentMatch = null;
			}
		}

		void BeginEncircling () {
			int performCount = currentMatch.GetPerformCount ();
			float time = currentMatch.Match.Settings.Duration * performCount;
			positioner.RotateAroundPoint (time);
		}*/

		protected virtual void OnInitPerformableTasks (PerformableTasks p) {}

		/*protected override void OnDisable () {
			base.OnDisable ();
			positioner.onArriveAtDestination -= OnArriveAtDestination;
			positioner.onEnterPoint -= OnEnterPoint;
			positioner.onExitPoint -= OnExitPoint;
		}*/

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