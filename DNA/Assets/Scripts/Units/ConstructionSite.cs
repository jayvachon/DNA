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
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new LaborGroup ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}
	}
}