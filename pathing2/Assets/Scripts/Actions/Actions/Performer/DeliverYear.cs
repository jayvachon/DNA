using UnityEngine;
using System.Collections;
using GameInventory;
using Units;

namespace GameActions {

	public class DeliverYear : DeliverItem<YearHolder> {

		public override System.Type RequiredPair {
			get { return null; }
		}

		public override bool CanPerform {
			get { return Inventory.Get<HealthHolder> ().Empty; }
		}
	}
}