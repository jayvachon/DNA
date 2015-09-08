using UnityEngine;
using System.Collections;
using GameInventory;
//using GameActions;
using DNA.Tasks;

namespace Units {

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

			//AcceptableActions = new AcceptableActions (this);
			//AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());

			AcceptableTasks.Add (new AcceptCollectItem<MilkshakeHolder> ());
		}

		void OnEmpty () {
			Destroy<MilkshakePool> ();
		}
	}
}