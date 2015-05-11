using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

// TODO: not needed?

namespace Units {

	public class House : StaticUnit {

		public override string Name {
			get { return "House"; }
		}
		
		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new ElderHolder (2, 1));

			AcceptableActions = new AcceptableActions (this);
			// AcceptableActions.Add (new AcceptCollectItem<ElderHolder> (new ElderCondition (true, true)));
			// AcceptableActions.Add (new AcceptDeliverItem<ElderHolder> (new ElderCondition (false, false)));
		}
	}
}