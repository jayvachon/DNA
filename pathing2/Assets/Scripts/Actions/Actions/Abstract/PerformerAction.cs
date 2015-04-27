using UnityEngine;
using System.Collections;

namespace GameActions {

	public abstract class PerformerAction : Action {

		public virtual System.Type RequiredPair { get { return null; } }
		public virtual bool CanPerform { get { return true; } }

		readonly bool autoStart;
		bool performing = false;
		protected bool autoRepeat = false;
		bool interrupt = false;

		protected float duration;
		public float Duration {
			get { return duration; }
			set { 
				// This is a hack - the constructor doesn't auto start if duration is the default value of -1
				// so, when the duration DOES get set, autos start happens here
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
		protected AcceptCondition AcceptCondition { get; private set; }
		protected PerformCondition PerformCondition { get; private set; }

		public PerformerAction (float duration=-1, bool autoStart=false, bool autoRepeat=false, PerformCondition performCondition=null) {
			this.duration = duration;
			this.autoStart = autoStart;
			this.autoRepeat = autoRepeat;
			this.PerformCondition = performCondition;
			if (duration != -1 && autoStart) {
				Start ();
			}
		}

		public void Bind (AcceptCondition acceptCondition) {
			AcceptCondition = acceptCondition;
		}

		public void Start (bool autoRepeat) {
			this.autoRepeat = autoRepeat;
			Start ();
		}

		public virtual void Start () {
			if (performing) return;
			performing = true;
			ActionHandler.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}

		public void End () {
			performing = false;
			if (PerformCondition == null || PerformCondition.CanPerform) {
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