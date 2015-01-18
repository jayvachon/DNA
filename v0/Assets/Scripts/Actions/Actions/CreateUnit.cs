using UnityEngine;
using System.Collections;

namespace GameActions {

	public class CreateUnit<T> : Action where T : MovableUnit {

		public CreateUnit (float duration) : base ("Create Unit", duration) {
			name = "Create " + typeof (T).Name;
		}

		public override void End () {
			PoolManager.instance.CreateUnit<T> (new Vector3 (2, 0.5f, -6));
			base.End ();
		}
	}
}
