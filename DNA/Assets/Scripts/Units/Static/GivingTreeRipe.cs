using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
using InventorySystem;

namespace DNA.Units {

	public class GivingTreeRipe : StaticUnit {

		public override string Name {
			get { return "Ripe Giving Tree"; }
		}

		public override string Description {
			get { return "To avoid drowning in the rising sea you can flee this tree and go to the next dimension."; }
		}

		void Awake () {
			/*PerformableActions.Add (new FleeTree (), "Flee Tree");*/
		}

		protected override void OnInitInventory (Inventory i) {
			Inventory.Add (new YearGroup (0, 500));
		}
	}
}