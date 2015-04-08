using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHappiness : CollectItem<HappinessHolder> {

		public override System.Type RequiredPair {
			get { return null; }
		}

		public CollectHappiness (float duration) : base (duration) {}
	}
}