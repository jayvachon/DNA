using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class MilkPool : StaticUnit, IInventoryHolder, IActionAcceptor {

		public Inventory Inventory { get; private set; }
		public AcceptableActions AcceptableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (100, 100));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectMilk", new AcceptCollectItem<MilkHolder> ());

			InventoryDrawer.Create (StaticTransform.transform, Inventory);
		}
	}
}