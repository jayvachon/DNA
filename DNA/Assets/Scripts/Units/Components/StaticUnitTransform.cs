using UnityEngine;
using System.Collections;
using Pathing;
using DNA.InputSystem;

namespace DNA.Units {

	public class StaticUnitTransform : UnitTransform {

		// TODO: deprecate
		void Start () {
			LookAtCenter ();
		}

		// TODO: deprecate
		public void LookAtCenter () {
			//MyTransform.LookAt (new Vector3 (0, -28.7f, 0), Vector3.up);
			//MyTransform.SetLocalEulerAnglesX (MyTransform.localEulerAngles.x - 90f);
		}
	}
}