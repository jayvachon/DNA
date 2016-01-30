#undef DEBUG_MSG
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class MobileUnit : Unit, ITaskPerformer, ISelectableOverrider, IPathElementVisitor {

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
				OnArriveAtDestination ((GridPoint)elem); // TODO: somtimes get an invalid cast exception - this needs to be handled
		}

		void OnArriveAtDestination (GridPoint point) {
			
			MyState = State.Idle;
			PathElement destination = TaskSelector.OnArrive (assignedElement);
			
			if (destination != null) {
				SetDestination (destination);
				return;
			}
		}

		void OnCompleteTask () {
			MyState = State.Idle;
		}

		void OnBeginTask () {
			MyState = State.Working;
			positioner.RotateAroundPoint (TaskSelector.PerformTime);
		}

		void OnBeginWait () {
			MyState = State.Waiting;
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

		class TaskSelection {

			public float PerformTime {
				get { return currentMatch.Match.Duration * currentMatch.GetPerformCount (); }
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

			public PathElement OnArrive (PathElement elem) {

				previousElem = currentElem;
				currentElem = elem;

				// Early out if the object associated with this PathElement does accept tasks
				if (!(elem.Object is ITaskAcceptor)) {
					return null;
				}
				
				// First, try to perform a task on this PathElement
				// Failing that, try to perform a task on an associated Connection or GridPoint

				if (!TryStartMatch (elem)) {
					
					GridPoint point = elem as GridPoint;
					if (point != null) {
						if (TryStartMatch (point.Connections.Find (x => x.State == DevelopmentState.UnderConstruction))) {
							currentElem = point as PathElement;
						}
					} else {
						Connection connection = elem as Connection;
						if (TryStartMatch (System.Array.Find (connection.Points, x => 
							x.State == DevelopmentState.UnderConstruction 
							|| x.State == DevelopmentState.Developed 
							|| x.State == DevelopmentState.UnderRepair))) {
							currentElem = connection as PathElement;
						}
					}
				}

				// If no task was found, search for a pair

				PathElement destination = null;
				/*if (result == TaskStartResult.Null || result == TaskStartResult.Disabled) {
					TryGetPairDestination (elem, out destination);
				}*/

				return destination;
			}

			public void InterruptTask () {
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Stop ();
				currentMatch = null;
			}

			bool TryStartMatch (PathElement elem) {

				if (elem != null) {

					MatchResult result;
					if (TryGetPrevious (elem, out result)) {
						StartMatch (result);
						return true;
					} else if (TryGetFromPoint (elem, out result)) {
						StartMatch (result);
						return true;
					}
				}

				return false;
			}

			void StartMatch (MatchResult match) {
				currentMatch = match;
				match.Match.onChangeState += OnChangeTaskState;
				match.Start (true);
			}

			void OnChangeTaskState (TaskState state) {
				switch (state) {
					case TaskState.Idle: currentMatch.Match.onChangeState -= OnChangeTaskState; break;
					case TaskState.Performing: 
						currentMatch.Match.onComplete += OnCompleteTask;
						unit.OnBeginTask (); 
						break;
					case TaskState.Queued: unit.OnBeginWait (); break;
				}
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
				unit.OnCompleteTask ();

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
			}
		}
		#endregion
	}
}