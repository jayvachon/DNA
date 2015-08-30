using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHealth : CollectItem<HealthHolder> {
		
		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new CollectHealthEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}