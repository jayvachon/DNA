using UnityEngine;
using System.Collections;
using DNA.Units;
using DNA.Paths;
using InventorySystem;

namespace DNA.Tasks {

	public class GenerateUnit : CostTask {
		public Unit GeneratedUnit { get; protected set; }
		public GenerateUnit (Inventory inventory=null) : base (inventory) {}
	}

	public class GenerateUnit<T> : GenerateUnit where T : Unit {

		public GenerateUnit (Inventory inventory=null) : base (inventory) {}

		protected override void OnEnd () {
			Purchase ();
			GeneratedUnit = UnitManager.Instantiate<T> ();
			base.OnEnd ();
		}
	}
}