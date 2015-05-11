using UnityEngine;
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
			StartMovingOnPath ();
		}

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			StartMovingOnPath ();
		}

		void StartMovingOnPath () {
			MobileTransform.StartMovingOnPath ();
			// PerformableActions.EnableAcceptedActionsBetweenAcceptors (
			// 	Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
			PerformableActions.PairActionsBetweenAcceptors (
				Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
		}

		public virtual void OnDragRelease (Unit unit) {}
	}
}