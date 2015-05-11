using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class ConsumeItemEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Empty; }
		}

		ItemHolder holder;

		public ConsumeItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}