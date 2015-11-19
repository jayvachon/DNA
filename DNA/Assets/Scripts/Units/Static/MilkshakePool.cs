using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class MilkshakePool : StaticUnit {
		
		void Awake () {
			unitRenderer.SetColors (new Color (0.294f, 0.741f, 0.847f));
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new MilkshakeGroup (100, 100));
			i["Milkshakes"].onEmpty += () => { Element.State = DevelopmentState.Abandoned; };
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<MilkshakeGroup> ());			
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Milkshakes"].Capacity = (int)(75 * Fertility.Multipliers[tier]);
			Inventory["Milkshakes"].Fill ();
		}

		protected override void OnSetState (DevelopmentState state) {
			base.OnSetState (state);
			if (state == DevelopmentState.Abandoned)
				Inventory["Milkshakes"].Clear ();
		}
	}
}