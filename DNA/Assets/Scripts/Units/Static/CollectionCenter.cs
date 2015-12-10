using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class CollectionCenter : StaticUnit {
		
		void Awake () {
			Inventory = Player.Instance.Inventory;
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptDeliverItem<MilkshakeGroup> ());
			a.Add (new AcceptDeliverItem<CoffeeGroup> ());
		}
	}
}