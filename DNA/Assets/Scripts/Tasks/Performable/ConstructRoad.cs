﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Models;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {

	public class ConstructRoad : CostTask, IConstructable {

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
					costSettings.Costs[0]["Milkshakes"] = Cost;
				return costSettings;
			}
		}

		public int Cost {
			get { return segmentCost * RoadConstructor.Instance.NewSegmentCount; }
		}

		public ConstructRoad (Inventory inventory=null) : base (inventory) {
			segmentCost = Settings.Costs[0]["Milkshakes"];
		}

		protected override void OnEnd () {
			Purchase ();
			List<Connection> connections = RoadConstructor.Instance.Connections;
			foreach (Connection c in connections) {
				ConstructionSite site = ConnectionsManager.GetContainer (c).BeginConstruction<Road> ();
				if (site != null) {
					site.LaborCost = segmentCost;
					site.RoadPlan = new RoadPlan (connections);
				}
			}
			RoadConstructor.Instance.Clear ();
			base.OnEnd ();
		}

		public bool CanConstruct (PathElement element) {
			return CanAfford 
				&& ((GridPoint)element).HasRoad 
				|| RoadConstructor.Instance.PointCount > 0;
		}
	}
}