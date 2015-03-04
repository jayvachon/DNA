using UnityEngine;
using System.Collections;
using Units;

namespace GameActions {

	public class GenerateUnit<T> : PerformerAction where T : MobileUnit {

		Vector3 position;

		public GenerateUnit (float duration) : base (duration, false, false) {}

		public override void OnEnd () {
			//UnitCreator.instance.Create<T> (position);
		}
	}
}