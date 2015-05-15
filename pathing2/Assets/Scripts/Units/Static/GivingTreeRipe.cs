using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class GivingTreeRipe : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Ripe Giving Tree"; }
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