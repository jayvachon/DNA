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

		public virtual void OnRelease () {
			//if (Path.Positioner.Moving) return;
			if (StartMovingOnPath (true)) {
				BoundAcceptor = null;
			} else {
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

		public virtual bool OnBindActionable (IActionAcceptor acceptor) {
			if (BoundAcceptor == acceptor) return false;
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
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
			BoundAcceptor = null;
			MobileTransform.Path.DragFromPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}
	}
}