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
			Inventory.Add (new MilkshakeHolder (75, 75));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());
		}
	}
}