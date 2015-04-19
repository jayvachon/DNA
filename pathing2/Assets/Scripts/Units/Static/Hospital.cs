using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Hospital : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Hospital"; }
		}

		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new ElderHolder (1, 0));
			Inventory.Add (new MilkshakeHolder (20, 5));
			Inventory.inventoryUpdated += OnInventoryUpdated;

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> (new ElderCondition (false, true)));
			AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> (new ElderCondition (true, false)));
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("ConsumeMilkshake", new ConsumeItem<MilkshakeHolder> (1, false));
			PerformableActions.Add ("HealElder", new HealElder (5, OnElderHealed));
		}

		bool IsSick (Item item) {
			ElderItem elder = item as ElderItem;
			return elder.HealthManager.Sick;
		}

		void OnInventoryUpdated () {
			bool hasSickElders = Inventory.Get<ElderHolder> ().Get (IsSick) != null;
			int milkshakeCount = Inventory.Get<MilkshakeHolder> ().Count;
			if (!hasSickElders || milkshakeCount < 5) return;
			PerformableActions.Start ("ConsumeMilkshake");
			PerformableActions.Start ("HealElder");
		}

		void OnElderHealed () {
			PerformableActions.Stop ("ConsumeMilkshake");
		}
	}
}