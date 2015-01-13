using UnityEngine;
using System.Collections;

namespace GameActions {

	public abstract class Action {

		float duration;
		public float Duration {
			get { return duration; }
		}

		bool autoRepeat = false;

		public IActionable Actionable { get; set; }

		public Action (float duration, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			if (autoStart) Start ();
		}

		public virtual void Start (IActionAcceptor acceptor=null) {
			ActionsManager.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}
		
		public virtual void End () {
			Actionable.OnEndAction ();
			if (autoRepeat) Start ();
		}
	}
}