using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class ConstructionSite : StaticUnit, IPathElementObject {

		public int LaborCost {
			get { return Inventory["Labor"].Count; }
			set { Inventory["Labor"].Set (value); }
		}

		public RoadPlan RoadPlan { get; set; }

		void Awake () {

			unitRenderer.SetColors (new Color (1f, 1f, 1f));

			Inventory = new Inventory (this);
			Inventory.Add (new LaborGroup ());
			AcceptableTasks.Add (new AcceptCollectItem<LaborGroup> ());
		}
	}
}