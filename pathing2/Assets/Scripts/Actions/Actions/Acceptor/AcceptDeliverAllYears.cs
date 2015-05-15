using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverAllYears : AcceptDeliverAllItems<YearHolder> {
		
		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptDeliverAllYearsEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}