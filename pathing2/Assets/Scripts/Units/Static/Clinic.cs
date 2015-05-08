using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Clinic : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Clinic"; }
		}

		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new BedHolder (3, 0, 0.1f));
			Inventory.Add (new MilkshakeHolder (10, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("OccupyBed", new AcceptOccupyBed ());
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("ConsumeMilkshake", new ConsumeItem<MilkshakeHolder> (-1, false)); 
			// TODO: Set rate based on # beds filled (and don't consume milkshakes if no beds are filled)
			// Also - don't slow elders' health rate unless there are milkshakes to consume
		}

		void OnInventoryUpdated () {
			//bool hasSickElders = Inventory.Get<ElderHolder> ().Get (IsSick) != null;
			//bool filledBeds = Inventory.Get<BedHolder> ();
			/*int milkshakeCount = Inventory.Get<MilkshakeHolder> ().Count;
			if (!hasSickElders || milkshakeCount < 5) return;
			PerformableActions.Start ("ConsumeMilkshake");
			PerformableActions.Start ("HealElder");*/
		}
	}
}