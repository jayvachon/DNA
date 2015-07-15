using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class GivingTreeRipe : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Ripe Giving Tree"; }
		}

		public override string Description {
			get { return "To avoid drowning in the rising sea you can flee this tree and go to the next dimension."; }
		}

		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 0));

			AcceptableActions = new AcceptableActions (this);
			
			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new FleeTree (), "Flee Tree");
		}
	}
}