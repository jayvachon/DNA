using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Distributor : MobileUnit, IActionPerformer {

		public override string Name {
			get { return "Distributor"; }
		}

		public PerformableActions PerformableActions { get; private set; }

		AgeManager ageManager = new AgeManager ();

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (3, 0));
			Inventory.Add (new ElderHolder (2, 0));

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (0.5f));
			PerformableActions.Add ("DeliverMilk", new DeliverItem<MilkHolder> (0.5f));
			PerformableActions.Add ("CollectIceCream", new CollectItem<IceCreamHolder> (1));
			PerformableActions.Add ("DeliverIceCream", new DeliverItem<IceCreamHolder> (1));
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> (2));
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> (2));
			PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (2));
			PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (2));
			PerformableActions.Add ("CollectUnitElder", new CollectUnit<Elder> (3));

			InventoryDrawer.Create (MobileTransform.transform, Inventory);

			ageManager.BeginAging (OnRetirement);
		}

		void OnRetirement () {
			// TODO: set name to "Elder"
			MobileTransform.StopMovingOnPath ();

			// When the distributor becomes an Elder, the player must click
			// and drag the distributor to a house

			// similarly, distributors must be dragged onto paths (paths are created
			// independently of distributors)
		}
	}
}