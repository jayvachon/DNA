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

			if (u != null) {

				// Ignore points that can't be reached
				GridPoint gp = u.Element as GridPoint;
				if (gp != null && !gp.HasRoad)
					return;

				AssignPoint (u.Element);
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

		protected Positioner2 positioner;
		MatchResult flowerMatch;

		MatchResult currentMatch;
		MatchResult previousMatch;
		PathElement currentElement;
		PathElement previousElement;

		TaskFinder taskFinder;
		TaskFinder TaskFinder {
			get {
				if (taskFinder == null)
					taskFinder = new TaskFinder (this);
				return taskFinder;
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

		TaskIndicator indicator;
		TaskIndicator Indicator {
			get {
				if (indicator == null) {
					indicator = ObjectPool.Instantiate<TaskIndicator> ();
					indicator.Parent = MyTransform;
					indicator.LocalScale = Vector3.one;
					indicator.LocalPosition = new Vector3 (0, 1.67f, 0);
				}
				return indicator;
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

		bool idle = true;
		public bool Idle { 
			get { return idle; }
			private set { 
				idle = value; 
				if (idle) Indicator.RemoveTask ();
			}
		}

		void AssignPoint (PathElement elem) {

			MatchResult match;
			if (TaskFinder.TaskFromPathElement (elem, out match)) {
				Indicator.SetTask (match.Match);
			} else {
				Indicator.RemoveTask ();
			}

			if (elem != currentElement) {
				StopMatch ();
				SetDestination (elem);
			}
		}

		void SetDestination (PathElement elem) {

			Idle = false;
			previousElement = currentElement;
			currentElement = elem;

			positioner.Destination = (currentElement is GridPoint)
				? (GridPoint)currentElement
				: System.Array.Find (((Connection)currentElement).Points, x => x.HasRoad);
		}

		bool FindMatch () {
			MatchResult match;
			PathElement destination;
			if (TaskFinder.TaskFromMatch (currentElement, previousMatch, out match)) {
				StartMatch (match);
				return true;
			} else if (TaskFinder.TaskFromPathElement (currentElement, out match)) {
				StartMatch (match);
				return true;
			} else if (TaskFinder.TaskFromConnections (currentElement, out destination, out match)) {
				if (destination != null) {
					SetDestination (destination);
				} else if (match != null) {
					StartMatch (match);
				}
				return true;
			} else if (TaskFinder.NearestPairFromDisabledTask (currentElement, out destination)) {
				SetDestination (destination);
				return true;
			}
			return false;
		}

		void StartMatch (MatchResult match) {
			
			previousMatch = currentMatch;
			currentMatch = match;
			Indicator.SetTask (currentMatch.Match);

			match.Match.onChangeState += OnChangeTaskState;
			match.Start (true);
		}

		public void StopMatch () {
			if (currentMatch != null) {
				currentMatch.Match.onComplete -= OnCompleteTask;
				currentMatch.Stop ();
			}
		}

		// Callbacks

		void OnArriveAtDestination (GridPoint point) {
			if (!FindMatch ()) {
				Idle = true;
			}
		}

		void OnChangeTaskState (TaskState state) {
			switch (state) {
				case TaskState.Idle: 
					currentMatch.Match.onChangeState -= OnChangeTaskState; 
					break;
				case TaskState.Performing: 
					currentMatch.Match.onComplete += OnCompleteTask;
					positioner.RotateAroundPoint (currentMatch.Duration);
					break;
				case TaskState.Queued: 
					// wait
					break;
			}
		}

		void OnCompleteTask (PerformerTask task) {
			
			task.onComplete -= OnCompleteTask;
			
			PathElement destination;
			if (TaskFinder.PairFromTask (task, currentElement, previousElement, out destination)) {
				SetDestination (destination);
			} else if (!FindMatch ()) {
				if (TaskFinder.NearestPathElementWithTask (currentElement, task, out destination)) {
					SetDestination (destination);
				} else {
					Idle = true;
				}
			} 
		}

		void OnEnterPoint (GridPoint point) {
			if (point.Unit is Flower) {
				flowerMatch = TaskMatcher.GetPerformable (this, point.Unit as ITaskAcceptor);
				if (flowerMatch != null) {
					flowerMatch.Start (true);
				}
			}
		}

		void OnExitPoint (GridPoint point) {
			if (flowerMatch != null) {
				flowerMatch.Stop ();
				flowerMatch = null;
			}
		}

		protected override void OnEnable () {
			base.OnEnable ();
			Events.instance.AddListener<ClickConnectionEvent> (OnClickConnection);
		}

		protected override void OnDisable () {
			base.OnDisable ();
			positioner.onArriveAtDestination -= OnArriveAtDestination;
			positioner.onEnterPoint -= OnEnterPoint;
			positioner.onExitPoint -= OnExitPoint;
			Events.instance.RemoveListener<ClickConnectionEvent> (OnClickConnection);
		}

		protected virtual void OnInitPerformableTasks (PerformableTasks p) {}

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
			Scale = Scale * 1.05f; // this ensures that the unit, when selected, shows up in front of any others it's overlapping
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			Scale = startScale;
		}

		void OnClickConnection (ClickConnectionEvent e) {
			if (e.Connection.Object == null)
				SelectionHandler.UnselectSingle (this);
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