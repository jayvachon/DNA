using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class MilkshakePool : StaticUnit {

		public override string Name {
			get { return "Milkshake Derrick"; }
		}

		public override string Description {
			get { return "Milkshakes collected from a Derrick can be used to construct buildings."; }
		}
		
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
			Inventory["Milkshakes"].Capacity = (int)(150 * Fertility.Multipliers[tier]);
			Inventory["Milkshakes"].Fill ();
		}
	}
}