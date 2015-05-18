using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Clinic : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Clinic"; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}

		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new ElderHolder (3, 0));
			Inventory.Get<ElderHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptDeliverUnpairedItem<ElderHolder> ());
		}
	}
}