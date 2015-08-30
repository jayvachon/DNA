using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectHappinessEnabledState : AcceptCollectItemEnabledState<HappinessHolder> {

		public override bool Enabled {
			get { return !holder.Empty; }
		}

		public override string RequiredPair {
			get { return ""; }
		}

		public override bool RequiresPair {
			get { return false; }
		}

		public AcceptCollectHappinessEnabledState (ItemHolder holder) : base (holder) {}
	}
}