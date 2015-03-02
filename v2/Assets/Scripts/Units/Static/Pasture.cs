using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Pasture : StaticUnit, IInventoryHolder, IActionAcceptor, IActionPerformer {

		public Inventory Inventory { get; private set; }
		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory ();
			Inventory.Add (new IceCreamHolder (5, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectIceCream", new AcceptCollectItem<IceCreamHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateIceCream", new GenerateItem<IceCreamHolder> (3));
			PerformableActions.Add ("ConsumeIceCream", new ConsumeItem<IceCreamHolder> (10));

			InventoryDrawer.Create (StaticTransform.transform, Inventory);
		}
	}
}