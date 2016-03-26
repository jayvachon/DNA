using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public abstract class UpgradeTask : CostTask {

		public bool UpgradeInProgress { get; set; }

		public override bool Enabled {
			get { return CanAfford && !UpgradeInProgress; }
		}

		public UpgradeTask (Inventory inventory=null) : base (inventory) {}
	}
}