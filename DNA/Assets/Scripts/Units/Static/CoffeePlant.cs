using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class CoffeePlant : StaticUnit {
		
		protected override void OnInitInventory (Inventory i) {
			i.Add (new CoffeeGroup ());
			i["Coffee"].onEmpty += () => { Element.State = DevelopmentState.Abandoned; };
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<CoffeeGroup> ());				
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Coffee"].Capacity = (int)(300 * Fertility.Multipliers[tier]);
			Inventory["Coffee"].Fill ();
		}

		protected override void OnSetState (DevelopmentState state) {
			base.OnSetState (state);
			if (state == DevelopmentState.Flooded) {
				Element.State = DevelopmentState.Abandoned;
				Inventory["Coffee"].Clear ();
			}
		}
	}
}