using UnityEngine;
using System.Collections;
using DNA.Models;
using DNA.InventorySystem;
using DNA.Paths;

namespace DNA.Tasks {

	public class BuildRoad : CostTask {

		int segmentCost;

		CostTaskSettings costSettings;
		public override CostTaskSettings Settings {
			get {
				if (costSettings == null) {
					try {
						costSettings = (CostTaskSettings)settings;
						segmentCost = costSettings.Costs["Milkshakes"]; // TODO: make this more robust (account for unknown resources)
					} catch {
						throw new System.Exception (this + " requires a CostTaskSettings model");
					}
				}
				costSettings.Costs["Milkshakes"] = segmentCost * RoadConstructor.Instance.NewSegmentCount;
				return costSettings;
			}
		}

		public BuildRoad (Inventory inventory=null) : base (inventory) {}

		protected override void OnEnd () {
			Purchase ();
			RoadConstructor.Instance.Build ();
			base.OnEnd ();
		}
	}
}