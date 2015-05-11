using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptOccupyBed : AcceptInventoryAction<BedHolder> {

		public override string Name {
			get { return "OccupyBed"; }
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptDeliverItemEnabledState (Holder);
				}
				return enabledState;
			}
		}
	}
}
