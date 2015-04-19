using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class House : StaticUnit {

		public override string Name {
			get { return "House"; }
		}
		
		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new ElderHolder (2, 1));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> (new ElderCondition (true, true)));
			AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> (new ElderCondition (false, false)));
		}
	}
}