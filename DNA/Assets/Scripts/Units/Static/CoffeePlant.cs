#undef NEVER_EMPTY
using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class CoffeePlant : StaticUnit, IWorkplace {

		public bool Accessible { get; set; }
		public float Efficiency { get; set; }
		
		protected override void OnInitInventory (Inventory i) {
			i.Add (new CoffeeGroup ());
			#if NEVER_EMPTY
				i["Coffee"].onRemove += () => { i["Coffee"].Add (); };
			#else
				i["Coffee"].onEmpty += () => { Element.State = DevelopmentState.Abandoned; };
			#endif
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new WorkplaceDeliverItem<CoffeeGroup> ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<CoffeeGroup> ());				
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Coffee"].Capacity = (int)(1500 * Fertility.Multipliers[tier]);
			Inventory["Coffee"].Fill ();
			TaskMatcher.StartMatch (this, Player.Instance);
		}

		protected override void OnSetState (DevelopmentState state) {
			base.OnSetState (state);
			if (state == DevelopmentState.Flooded) {
				Element.State = DevelopmentState.Abandoned;
				Inventory["Coffee"].Clear ();
			}
		}

		protected override void OnUpdateAccessibility () {
			if (Accessible)
				TaskMatcher.StartMatch (this, Player.Instance);
		}

		public void OnUpdateEfficiency () {
			TaskMatcher.StartMatch (this, Player.Instance);
		}
	}
}