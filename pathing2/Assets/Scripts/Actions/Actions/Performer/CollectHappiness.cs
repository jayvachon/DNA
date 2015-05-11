using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHappiness : CollectItem<HappinessHolder> {
		
		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new CollectHappinessEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}