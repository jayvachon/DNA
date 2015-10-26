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

			Inventory = new Inventory (this);
			//Inventory.Add (new YearHolder (500, 0));
			Inventory.Add (new YearGroup (0, 500));

			/*PerformableActions.Add (new FleeTree (), "Flee Tree");*/
		}
	}
}