#undef NEVER_EMPTY
using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	// TODO: rename to MilkshakeDerrick

	public class MilkshakePool : StaticUnit {
		
		protected override void OnInitInventory (Inventory i) {
			i.Add (new MilkshakeGroup (100, 100));
			#if NEVER_EMPTY
				i["Milkshakes"].onRemove += () => { i["Milkshakes"].Add (); };
			#else
				i["Milkshakes"].onEmpty += () => { Element.State = DevelopmentState.Abandoned; };
			#endif
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<MilkshakeGroup> ());			
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Milkshakes"].Capacity = (int)(1000 * Fertility.Multipliers[tier]);
			Inventory["Milkshakes"].Fill ();
		}

		protected override void OnSetState (DevelopmentState state) {
			base.OnSetState (state);
			if (state == DevelopmentState.Abandoned) {
				Inventory["Milkshakes"].Clear ();
			} else if (state == DevelopmentState.Flooded) {
				AcceptableTasks.DeactivateAll ();
			} else {
				AcceptableTasks.ActivateAll ();
			}
		}
	}
}