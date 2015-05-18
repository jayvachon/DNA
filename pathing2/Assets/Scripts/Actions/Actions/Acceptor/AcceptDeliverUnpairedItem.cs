using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverUnpairedItem<T> : AcceptDeliverItem<T> where T : ItemHolder {

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptDeliverUnpairedItemEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}