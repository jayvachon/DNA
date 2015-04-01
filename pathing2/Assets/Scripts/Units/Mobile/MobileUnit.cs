using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;
using GameInput;

namespace Units {

	public class MobileUnit : Unit, IBinder {

		MobileUnitTransform mobileTransform;
		protected MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = unitTransform as MobileUnitTransform;
				}
				return mobileTransform;
			}
		}

		public Path Path {
			get { 
				IPathable pathable = MobileTransform as IPathable; 
				return pathable.Path;
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

		public override void OnSelect () {
			base.OnSelect ();
			LayerController.Layer = InputLayer.PathPoints;
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			LayerController.Reset ();
		}

		public virtual void OnDragRelease (Unit unit) {}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				MobileTransform.StartMovingOnPath ();
			}
		}
	}
}