﻿using UnityEngine;
using System.Collections;
using DNA.Models;
using DNA.InventorySystem;
using DNA.Paths;

namespace DNA.Tasks {

	public class BuildRoad : CostTask, IConstructable {

		// TODO: this whole segment deal is super hacky
		int segmentCost = -1;
		
		CostTaskSettings costSettings;
		public override CostTaskSettings Settings {
			get {
				if (costSettings == null) {
					try {
						costSettings = (CostTaskSettings)settings;
					} catch {
						throw new System.Exception (this + " requires a CostTaskSettings model");
					}
				}
				if (segmentCost > -1)
					costSettings.Costs["Milkshakes"] = Cost;
				return costSettings;
			}
		}

		public int Cost {
			get { return segmentCost * RoadConstructor.Instance.NewSegmentCount; }
		}

		public BuildRoad (Inventory inventory=null) : base (inventory) {
			segmentCost = Settings.Costs["Milkshakes"];
		}

		protected override void OnEnd () {
			Purchase ();
			RoadConstructor.Instance.Build ();
			base.OnEnd ();
		}

		public bool CanConstructOnPoint (GridPoint point) {
			return CanAfford && point.HasRoad || RoadConstructor.Instance.PointCount > 0;
		}
	}
}