using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Distributor : MobileUnit, IInventoryHolder, IActionPerformer {

		public Inventory Inventory { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (3, 0));
			Inventory.Add (new ElderHolder (2, 0));

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (3));
			PerformableActions.Add ("CollectIceCream", new CollectItem<IceCreamHolder> (3));
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> (3));
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> (3));
			PerformableActions.Add ("CollectItem", new CollectItem<ElderHolder> (3));
			PerformableActions.Add ("DeliverItem", new DeliverItem<ElderHolder> (3));
		}
	}
}