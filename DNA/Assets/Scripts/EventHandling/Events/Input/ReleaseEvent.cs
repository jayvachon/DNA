using UnityEngine;
using System.Collections;
using DNA.InputSystem;

namespace DNA.EventSystem {
	
	public class ReleaseEvent : GameEvent {

		public readonly ReleaseSettings releaseSettings;

		public ReleaseEvent (ReleaseSettings releaseSettings) {
			this.releaseSettings = releaseSettings;
		}
	}
}