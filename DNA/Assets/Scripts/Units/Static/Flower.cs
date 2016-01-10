using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Flower : StaticUnit {

		protected override void OnInitInventory (Inventory i) {
			i.Add (new HappinessGroup (200, 200));
			i["Happiness"].onUpdate += OnCollectHappiness;
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<HappinessGroup> ());
		}

		void OnCollectHappiness () {
			Inventory["Happiness"].Fill ();
		}
	}
}