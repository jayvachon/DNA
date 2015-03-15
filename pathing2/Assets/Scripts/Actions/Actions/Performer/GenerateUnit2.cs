using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace GameActions {

	public class GenerateUnit2<T> : PerformerAction where T : Unit {

		Vector3 createPosition;

		public GenerateUnit2 (Vector3 createPosition, MilkshakeHolder holder) : base (0, false, false) {
			this.createPosition = createPosition;
		}

		public override void OnEnd () {
			Unit unit = ObjectCreator.Instance.Create<T> ().GetScript<Unit> ();
			unit.Position = createPosition;
		}
	}
}