using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnit : Unit {

		MobileUnitTransform mobileTransform;
		MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = unitTransform as MobileUnitTransform;
				}
				return mobileTransform;
			}
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				MobileTransform.StartMoveOnPath ();
			}
		}
	}
}