using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverUnpairedItemEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}
		
		ItemHolder holder;

		public AcceptDeliverUnpairedItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}