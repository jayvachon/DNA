using UnityEngine;
using System.Collections;

namespace GameActions {

	public abstract class PerformerAction : Action {

		readonly bool autoStart;
		bool performing = false;
		protected bool autoRepeat = false;
		bool interrupt = false;

		float efficiency = 1f; // Percentage
		public float Efficiency {
			get { return efficiency; }
			set { efficiency = value; }
		}

		protected float duration;
		public float Duration {
			get { return duration * Efficiency; }
			set { 
				// This is a hack - the constructor doesn't auto start if duration is the default value of -1
				// so, when the duration DOES get set, auto start happens here
				// a better way of doing this would be to have classes that inherit from PerformerAction set their name
				// in their constructors so that PerformerAction can access TimeValues.ActionTimes
				float prevValue = duration;
				if (value != -1) {
					duration = value;
					if (prevValue == -1 && autoStart) {
						Start ();
					}
				}
			}
		}

		public IActionPerformer Performer { get; set; }

		public PerformerAction (float duration=-1, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoStart = autoStart;
			this.autoRepeat = autoRepeat;
			if (duration != -1 && autoStart) {
				Start ();
			}
		}

		public void Start (bool autoRepeat) {
			this.autoRepeat = autoRepeat;
			Start ();
		}

		public virtual void Start () {
			if (!Enabled) return;
			if (duration == 0) {
				End ();
				return;
			}
			if (performing) return;
			performing = true;
			ActionHandler.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}

		public void End () {
			performing = false;
			if (Enabled) {
				OnEnd ();
			}
			if (autoRepeat) {
				if (interrupt) {
					interrupt = false;
				} else {
					Start ();
				}
			}
		}

		public virtual void OnEnd () {}

		public void Stop () {
			interrupt = true;
		}
	}
}