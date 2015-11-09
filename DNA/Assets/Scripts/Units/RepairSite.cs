using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class RepairSite : StaticUnit, IPathElementObject {

		public int LaborCost {
			get { return Inventory["Labor"].Count; }
			set { Inventory["Labor"].Set (value); }
		}

		void Awake () {
			unitRenderer.SetColors (Palette.White);
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new LaborGroup ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}
	}
}