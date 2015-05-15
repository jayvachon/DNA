using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHappinessEnabledState : CollectItemEnabledState<HappinessHolder> {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		public override string RequiredPair {
			get { return ""; }
		}

		public CollectHappinessEnabledState (ItemHolder holder) : base (holder) {}
	}
}