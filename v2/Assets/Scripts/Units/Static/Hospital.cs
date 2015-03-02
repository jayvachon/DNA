﻿using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Hospital : StaticUnit, IInventoryHolder, IActionAcceptor, IActionPerformer {

		public Inventory Inventory { get; private set; }
		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory ();
			Inventory.Add (new ElderHolder (10, 0));
			Inventory.Add (new MilkshakeHolder (20, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> (new ElderCondition (false, false)));
			AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> (new ElderCondition (true, true)));
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("ConsumeMilkshake", new ConsumeItem<MilkshakeHolder> (3));

			InventoryDrawer.Create (StaticTransform.transform, Inventory);
		}
	}
}