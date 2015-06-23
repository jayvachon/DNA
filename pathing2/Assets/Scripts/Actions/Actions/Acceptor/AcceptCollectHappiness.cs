using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectHappiness : AcceptCollectItem<HappinessHolder> {
		
		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptCollectHappinessEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}