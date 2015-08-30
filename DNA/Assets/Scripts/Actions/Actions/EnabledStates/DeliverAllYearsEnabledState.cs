using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverAllYearsEnabledState<T> : EnabledState where T : ItemHolder {

		public override bool Enabled {
			get { return !holder.Empty; }
		}
		
		ItemHolder holder;

		public DeliverAllYearsEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}