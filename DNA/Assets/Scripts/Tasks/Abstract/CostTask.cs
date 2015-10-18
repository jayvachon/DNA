using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Models;

namespace DNA.Tasks {

	// Task that costs resources
	public abstract class CostTask : InventoryTask {

		CostTaskSettings costSettings;
		new public virtual CostTaskSettings Settings {
			get {
				if (costSettings == null) {
					try {
						costSettings = (CostTaskSettings)settings;
					} catch {
						throw new System.Exception (this + " requires a CostTaskSettings model");
					}
				}
				return costSettings;
			}
		}

		public override bool Enabled {
			get { return CanAfford; }
		}

		protected bool CanAfford {
			get {
				foreach (var cost in Settings.Costs) {
					try {
						if (Inventory[cost.Key].Count < cost.Value)
							return false;
					} catch {
						throw new System.Exception ("The task '" + this + "' requires an inventory with '" + cost.Key + "'");
					}
				}
				return true;
			}
		}

		int totalCost = 0;
		protected int TotalCost {
			get {
				if (totalCost == 0) {
					foreach (var cost in Settings.Costs) {
						totalCost += Inventory[cost.Key].Count;
					}
				}
				return totalCost;
			}
		}

		public CostTask (Inventory inventory=null) : base (inventory) {}

		protected void Purchase () {
			foreach (var cost in Settings.Costs)
				Inventory[cost.Key].Remove (cost.Value);
		}
	}
}