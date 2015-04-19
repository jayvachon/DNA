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
			PerformableActions.Add ("ConsumeMilkshake", new ConsumeItem<MilkshakeHolder> (5));
		}
	}
}