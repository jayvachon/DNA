using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Tasks;

namespace DNA.Units {

	public class MilkshakePool : StaticUnit {

		public override string Name {
			get { return "Milkshake Derrick"; }
		}

		public override string Description {
			get { return "Milkshakes collected from a Derrick can be used to construct buildings."; }
		}
		
		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeHolder (50, 50));
			Inventory.Get<MilkshakeHolder> ().HolderEmptied += OnEmpty;

			AcceptableTasks.Add (new AcceptCollectItem<MilkshakeHolder> ());
		}

		void OnEmpty () {
			Destroy<MilkshakePool> ();
		}
	}
}