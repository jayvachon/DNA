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

		public PerformableActions PerformableActions { get; protected set; }
		public IActionAcceptor BoundAcceptor { get; protected set; } //TODO: should be private set

		enum State {
			Bound, Moving
		}

		State state;

		bool moveOnRelease = true;

		public void Init (IActionAcceptor givingTree) {
			BoundAcceptor = givingTree;
		}

		public override void OnSelect () {
			base.OnSelect ();
			Events.instance.AddListener<ClickEvent> (OnClickEvent);
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			Events.instance.RemoveListener<ClickEvent> (OnClickEvent);
		}

		public virtual void OnRelease () {
			/*if (moveOnRelease && StartMovingOnPath (true)) {
				BoundAcceptor = null;
				moveOnRelease = false;
			} else if (Path.Points.Count < 2) {
				BindToCollider ();
			}*/
		}

		protected void BindToCollider () {
			/*UnitClickable clickable = null;
			try {
				clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits);
			} catch (NullReferenceException e) {
				Debug.LogError (Name + " does not reference its MobileClickable. Assign it in the inspector. \n" + e);
			}
			if (clickable != null) {
				if (OnBindActionable (clickable.StaticUnit as IActionAcceptor)) {
					OnBind ();
				}
			} else {
				// disgusting hack that prevents an elder from being unbound from a clinic
				// unless it has been moved some distance away from the clinic
				// INSTEAD: units should have States (IDLE, MOVING, TAKING ACTION) 
				if (BoundAcceptor != null) {
					StaticUnit su = (StaticUnit)BoundAcceptor;
					if (Vector3.Distance (Position, su.Position) <= 1.5f) return;
				}
				BoundAcceptor = null;
				OnUnbind ();
			}*/
		}

		protected virtual void OnBind () {}
		protected virtual void OnUnbind () {}

		// Returns true if this is a newly bound acceptor
		public virtual PerformerAction OnBindActionable (IActionAcceptor acceptor) {
			if (BoundAcceptor == acceptor) return null; //return false;
			BoundAcceptor = acceptor;
			return ActionHandler.instance.Bind (this);
			/*PerformerAction action = ActionHandler.instance.Bind (this);
			if (action != null) {
				MobileTransform.EncircleBoundUnit (action);
			}
			return true;*/
		}

		public virtual void OnEndActions () {
			if (gameObject.activeSelf) StartCoroutine (CoWaitForCompleteCircle ());
		}

		IEnumerator CoWaitForCompleteCircle () {
			/*while (MobileTransform.Circling) {
				yield return null;
			}*/
			while (MobileTransform.Working) {
				yield return null;
			}
			StartMovingOnPath ();
		}

		bool StartMovingOnPath (bool reset=false) {
			PerformableActions.PairActionsBetweenAcceptors (
				Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
			return MobileTransform.StartMovingOnPath (reset);
		}

		public void OnDragEnter () {
			moveOnRelease = true;
			MobileTransform.Path.DragFromPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}

		void OnClickEvent (ClickEvent e) {
			if (e.left) return;
			UnitClickable unit = e.GetClickedOfType<UnitClickable> ();
			if (unit == null) return;
			Path.Points.Clear ();
			Path.Points.Add (((StaticUnit)BoundAcceptor).PathPoint);
			Path.Points.Add (unit.StaticUnit.PathPoint);
			MobileTransform.StartMovingOnPath (false);

			/*UnitClickable unit = e.GetClickedOfType<UnitClickable> ();
			if (unit == null) return;
			if (unit.StaticUnit as IActionAcceptor == BoundAcceptor) {
				// TODO: not working (mobile doesn't carry out the action)
				OnBindActionable (BoundAcceptor);
				return;
			}
			AcceptorAction a = unit.StaticUnit.AcceptableActions.GetEnabledAction ();
			if (a != null && PerformableActions.HasMatchingAction (a)) {
				Path.Points.Clear ();
				Path.Points.Add (((StaticUnit)BoundAcceptor).PathPoint);
				Path.Points.Add (unit.StaticUnit.PathPoint);
				MobileTransform.StartMovingOnPath (false);
			}*/
		}
	}
}