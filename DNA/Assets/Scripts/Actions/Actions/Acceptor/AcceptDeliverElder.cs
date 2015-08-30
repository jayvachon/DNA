using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	// TODO: Deprecate
	public class AcceptDeliverElder : AcceptDeliverItem<ElderHolder> {

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					//enabledState = new AcceptDeliverElderEnabledState (Holder);
					enabledState = new AcceptDeliverUnpairedItemEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}