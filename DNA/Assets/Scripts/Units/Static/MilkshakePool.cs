using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
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

			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeGroup (100, 100));
			Inventory["Milkshakes"].onEmpty += () => { Element.State = DevelopmentState.Abandoned; };
			//Inventory.Add (new MilkshakeHolder (100, 100));
			//Inventory.Get<MilkshakeHolder> ().HolderEmptied += () => { Element.State = DevelopmentState.Abandoned; };

			//AcceptableTasks.Add (new AcceptCollectItem<MilkshakeHolder> ());
			AcceptableTasks.Add (new AcceptCollectItem<MilkshakeGroup> ());
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Milkshakes"].Capacity = (int)(150 * Fertility.Multipliers[tier]);
			Inventory["Milkshakes"].Fill ();
			//Inventory["Milkshakes"].Initialize ();
			Debug.Log (Inventory["Milkshakes"].Capacity);
		}
	}
}