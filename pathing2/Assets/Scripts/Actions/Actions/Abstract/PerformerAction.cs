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
			set { duration = value; }
		}

		IActionPerformer performer;
		public IActionPerformer Performer { 
			get { return performer; }
			set { 
				bool first = (performer == null);
				performer = value;
				if (first && autoStart) {
					Start ();
				}
			}
		}

		public PerformerAction (float duration=-1, bool autoStart=false, bool autoRepeat=false) {
			this.duration = (duration == -1) ? TimerValues.GetActionTime (Name) : duration;
			this.autoStart = autoStart;
			this.autoRepeat = autoRepeat;
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