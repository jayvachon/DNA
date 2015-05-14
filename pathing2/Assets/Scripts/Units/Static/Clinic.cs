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
			Inventory.Add (new ElderHolder (3, 0));
			Inventory.Get<ElderHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);
			//Inventory.Add (new BedHolder (3, 0, 0.1f));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptDeliverElder ());
			//AcceptableActions.Add (new AcceptOccupyBed ());
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