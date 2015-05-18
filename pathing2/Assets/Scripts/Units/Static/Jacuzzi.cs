using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Jacuzzi : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Jacuzzi"; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new HappinessHolder (100, 100));
			Inventory.Add (new DistributorHolder (3, 0));
			Inventory.Get<DistributorHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<HappinessHolder> ());
			AcceptableActions.Add (new AcceptDeliverUnpairedItem<DistributorHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<HappinessHolder> ());
		}
	}
}