using UnityEngine;
using System.Collections;
using Units;

namespace GameActions {

	public class GenerateUnit<T> : PerformerAction where T : Unit {

		Vector3 createPosition;

		public GenerateUnit (float duration, Vector3 createPosition) : base (duration, false, false) {
			this.createPosition = createPosition;
		}

		public override void OnEnd () {
			ObjectCreator.Instance.Create<Distributor> (createPosition);
		}
	}
}