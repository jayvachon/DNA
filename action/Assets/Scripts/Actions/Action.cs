using UnityEngine;
using System.Collections;

namespace GameActions {

	public class Action {

		float duration;
		public float Duration {
			get { return duration; }
		}

		bool autoRepeat = false;

		public Action (float duration, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			if (autoStart) Start ();
		}

		public virtual void Start () {
			ActionsManager.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}
		public virtual void End () {
			if (autoRepeat) Start ();
		}
		
		/*public virtual void OnStartAction (IActionable point, IActionable visitor) {
			point.OnArrive ();
			visitor.OnArrive ();
		}

		public virtual void PerformAction (float progress, IActionable visitor) {}
		public virtual void OnEndAction (IActionable point, IActionable visitor) {
			point.OnDepart ();
			visitor.OnDepart ();
		}*/
	}
}