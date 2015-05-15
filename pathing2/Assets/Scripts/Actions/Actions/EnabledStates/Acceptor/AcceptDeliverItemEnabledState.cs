using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverItemEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		ItemHolder holder;

		public AcceptDeliverItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}