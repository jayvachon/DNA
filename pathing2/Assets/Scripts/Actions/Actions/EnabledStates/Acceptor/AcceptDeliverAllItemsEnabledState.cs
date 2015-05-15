using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverAllItemsEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		ItemHolder holder;

		public AcceptDeliverAllItemsEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}