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
					healthManager = Elder.HealthManager;
				}
				return healthManager;
			}
		}

		bool ShouldStartSickness {
			get { return Random.value < 0.2f; }
		}

		Stopwatch deathTime = new Stopwatch ();

		public SubtractHealth () : base (5f, true, false, null) {}

		public override void Start () {
			deathTime.Begin ();
			base.Start ();
		}

		public override void OnEnd () {
			if (ShouldStartSickness) {
				HealthManager.StartSickness ();
			}

			// The first time through, the timer waits 15 seconds. 
			// Every time after, timer waits 5 seconds
			if (!autoRepeat) {
				autoRepeat = true;
				duration = 5f;
			}
		}
	}
}