using UnityEngine;
using System.Collections;

namespace GameActions {
	
	public abstract class Action : INameable {

		public virtual string Name {
			get { return ""; }
		}

		float duration;
		public float Duration {
			get { return duration; }
		}

		bool autoRepeat = false;
		public IActionPerformer Performer { get; set; }

		public Action (float duration, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			if (autoStart) Start ();
		}

		public virtual void Start () {
			ActionHandler.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}

		public virtual void End () {
			Performer.OnEndAction ();
			if (autoRepeat) Start ();
		}
	}
}