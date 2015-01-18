using UnityEngine;
using System.Collections;

namespace GameActions {

	public class CreateUnit<T> : Action where T : MovableUnit {

		string name = "Create Unit";
		public override string Name {
			get { return name; }
		}

		public CreateUnit (float duration) : base (duration) {
			name = "Create " + typeof (T).Name;
		}

		public override void End () {
			PoolManager.instance.CreateUnit<T> (new Vector3 (2, 0.5f, -6));
			base.End ();
		}
	}
}
