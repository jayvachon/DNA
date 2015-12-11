using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Models;
using InventorySystem;

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
			get { 
				if (Settings.Symbol == "construct_flower")
					Debug.Log (CanAfford);
				return CanAfford; }
		}

		public virtual int Level { get; protected set; }

		protected bool CanAfford {
			get {
				foreach (var cost in Settings.Costs[Level]) {
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
		public int TotalCost {
			get {
				if (totalCost == 0) {
					foreach (var cost in Settings.Costs[Level])
						totalCost += cost.Value;
				}
				return totalCost;
			}
		}

		public Dictionary<string, int> Costs {
			get { return Settings.Costs[Level]; }
		}

		public CostTask (Inventory inventory=null) : base (inventory) {}

		protected void Purchase () {
			foreach (var cost in Settings.Costs[Level])
				Inventory[cost.Key].Remove (cost.Value);
			if (Level < Settings.Costs.Length-1)
				Level ++;
		}
	}
}