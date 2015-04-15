using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class SubtractHealth : PerformerAction {

		ElderItem elder = null;
		ElderItem Elder {
			get {
				if (elder == null) {
					elder = Performer as ElderItem;
				}
				return elder;
			}
		}

		HealthManager healthManager = null;
		HealthManager HealthManager {
			get {
				if (healthManager == null) {
					healthManager = elder.HealthManager;
				}
				return healthManager;
			}
		}

		// public SubtractHealth (float duration) : base (duration, true, true, null) {}
		public SubtractHealth () : base (15f, true, false, null) {}

		public override void OnEnd () {
			//Elder.SubtractHealth (0.1f);
			

			// The first time through, the timer waits 15 seconds. 
			// Every time after, timer waits 5 seconds
			if (!autoRepeat) {
				autoRepeat = true;
				duration = 5f;
				Start ();
			}
		}
	}
}