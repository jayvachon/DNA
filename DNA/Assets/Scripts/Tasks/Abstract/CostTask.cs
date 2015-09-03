using UnityEngine;
using System.Collections;
using GameInventory;
using DNA.Models;

namespace DNA.Tasks {

	// surely there's a better name for this (tasks that cost resources)
	public abstract class CostTask: InventoryTask {

		CostTaskSettings costSettings;
		new public CostTaskSettings Settings {
			get {
				if (costSettings == null) {
					try {
						costSettings = (CostTaskSettings)settings;
					} catch {
						throw new System.Exception (this + "' requires a CostTaskSettings model");
					}
				}
				return costSettings;
			}
		}

		public override bool Enabled {
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

		void Purchase () {
			foreach (var cost in Settings.Costs)
				Inventory[cost.Key].Remove (cost.Value);
		}
	}
}