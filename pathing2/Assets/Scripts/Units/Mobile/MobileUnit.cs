using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;
using GameInput;

namespace Units {

	public class MobileUnit : Unit, IBinder {

		MobileUnitTransform mobileTransform;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = unitTransform as MobileUnitTransform;
				}
				return mobileTransform;
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

		public IActionAcceptor BoundAcceptor { get; private set; }

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			mobileTransform.StartMovingOnPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}
		public virtual void OnRelease () {}
	}
}