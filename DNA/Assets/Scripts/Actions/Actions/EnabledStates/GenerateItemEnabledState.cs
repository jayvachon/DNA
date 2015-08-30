using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItemEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		ItemHolder holder;

		public GenerateItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}