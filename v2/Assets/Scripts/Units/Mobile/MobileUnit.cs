using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

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

		public IActionAcceptor BoundAcceptor { get; private set; }

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			mobileTransform.StartMoveOnPath ();
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				MobileTransform.StartMoveOnPath ();
			}
		}
	}
}