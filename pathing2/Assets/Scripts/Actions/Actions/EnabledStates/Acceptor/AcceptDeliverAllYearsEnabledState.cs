using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverAllYearsEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		ItemHolder holder;

		public AcceptDeliverAllYearsEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}