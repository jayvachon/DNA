using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace DNA.Tasks {

	public class GenerateUnit : CostTask {
		public Unit GeneratedUnit { get; protected set; }
		public GenerateUnit (Inventory inventory=null) : base (inventory) {}
	}

	public class GenerateUnit<T> : GenerateUnit where T : Unit {

		public GenerateUnit (Inventory inventory=null) : base (inventory) {}

		protected override void OnEnd () {
			Purchase ();
			GeneratedUnit = ObjectCreator.Instance.Create<T> ().GetScript<Unit> ();
			base.OnEnd ();
		}
	}
}