using UnityEngine;
using System.Collections;

namespace GameActions {

	// TODO: rewrite so that there are two types of PerformerActions: ones that must be explicitly started
	// and ones that loop (or is there a better division?)
	public abstract class PerformerAction : Action {

		readonly bool autoStart;
		protected bool autoRepeat = false;
		bool interrupt = false;
		
		bool performing = false;
		public bool Performing {
			get { return performing; }
		}

		bool active = true;
		public override bool Active {
			get { return active; }
			set {
				active = value;
				if (autoStart) {
					Start ();
				}
			}
		}

		public float Progress { get; set; }

		float efficiency = 1f; // Percentage
		public float Efficiency {
			get { return efficiency; }
			set { efficiency = value; }
		}

		protected float duration;
		public float Duration {
			get { 
				#if VARIABLE_TIME
				float t = TimerValues.Instance.GetActionTime (Name);
				if (t != -1) return t * Efficiency;
				return duration * Efficiency;
				#endif
				return duration * Efficiency; 
			}
			set { duration = value; }
		}

		IActionPerformer performer;
		public IActionPerformer Performer { 
			get { return performer; }
			set { 
				bool initial = (performer == null);
				performer = value;
				if (initial && autoStart) {
					Start ();
				}
			}
		}

		public PerformerAction (float duration=-1, bool autoStart=false, bool autoRepeat=false) {
			this.duration = (duration == -1) 
				? TimerValues.Instance.GetActionTime (Name)
				: duration;
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

		public void BindStart () {
			performing = true;
		}

		public void BindEnd () {
			performing = false;
			if (Enabled) OnEnd ();
		}

		public virtual void OnEnd () {}

		public virtual void Stop () {
			interrupt = true;
		}
	}
}