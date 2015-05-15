using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectItemEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Empty; }
		}

		ItemHolder holder;

		public AcceptCollectItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}