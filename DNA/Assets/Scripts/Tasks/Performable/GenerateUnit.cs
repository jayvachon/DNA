using UnityEngine;
using System.Collections;
using DNA.Units;
using DNA.Paths;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class GenerateUnit : CostTask {
		public Unit GeneratedUnit { get; protected set; }
		public GenerateUnit (Inventory inventory=null) : base (inventory) {}
	}

	public class GenerateUnit<T> : GenerateUnit, IConstructable where T : Unit {

		public GenerateUnit (Inventory inventory=null) : base (inventory) {}

		protected override void OnEnd () {
			Purchase ();
			GeneratedUnit = ObjectPool.Instantiate<T> ();
			base.OnEnd ();
		}

		public bool CanConstructOnPoint (GridPoint point) {
			return CanAfford && point.HasRoad && point.Object.GetType () == typeof (DrillablePlot);
		}
	}
}