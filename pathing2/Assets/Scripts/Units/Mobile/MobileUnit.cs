using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;
using GameInput;

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
		public IActionAcceptor BoundAcceptor { get; private set; }

		bool moveOnRelease = true;

		public virtual void OnRelease () {
			if (moveOnRelease && StartMovingOnPath (true)) {
				BoundAcceptor = null;
				moveOnRelease = false;
			} else if (Path.Points.Count < 2) {
				BindToCollider ();
			}
		}

		protected void BindToCollider () {
			UnitClickable clickable = null;
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
				BoundAcceptor = null;
				OnUnbind ();
			}
		}

		protected virtual void OnBind () {}
		protected virtual void OnUnbind () {}

		// Returns true if this is a newly bound acceptor
		public virtual bool OnBindActionable (IActionAcceptor acceptor) {
			//Debug.Log (BoundAcceptor + " , new: " + acceptor);
			if (BoundAcceptor == acceptor) return false;
			BoundAcceptor = acceptor;
			PerformerAction action = ActionHandler.instance.Bind (this);
			if (action != null) {
				MobileTransform.EncircleBoundUnit (action);
			}
			return true;
		}

		public virtual void OnEndActions () {
			StartMovingOnPath ();
		}

		bool StartMovingOnPath (bool reset=false) {
			PerformableActions.PairActionsBetweenAcceptors (
				Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
			return MobileTransform.StartMovingOnPath (reset);
		}

		public void OnDragEnter () {
			moveOnRelease = true;
			BoundAcceptor = null;
			MobileTransform.Path.DragFromPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}
	}
}