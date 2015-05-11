using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class MilkshakePool : StaticUnit {

		public override string Name {
			get { return "Milkshake Derrick"; }
		}
		
		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeHolder (100, 100));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());
		}
	}
}