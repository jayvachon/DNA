using UnityEngine;
using System.Collections;

namespace GameActions {

	public class AcceptOccupyUnit : AcceptorAction {

		public override string Name {
			get { return "OccupyUnit"; }
		}
		
		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptOccupyUnitEnabledState ();
				}
				return enabledState;
			}
		}
	}
}