using UnityEngine;
using System.Collections;

namespace GameActions {

	public class GenerateUnit<T> : PerformerAction where T : Unit {

		Vector3 position;

		public GenerateUnit (float duration) : base (duration, false, false) {}

		public void StartGenerate (Vector3 position) {
			this.position = position;
			Start ();
		}

		public override void OnEnd () {
			UnitCreator.instance.Create<T> (position);
		}
	}
}