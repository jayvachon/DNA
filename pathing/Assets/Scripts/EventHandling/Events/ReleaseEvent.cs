using UnityEngine;
using System.Collections;
using GameInput;

namespace GameEvents {
	
	public class ReleaseEvent : GameEvent {

		public readonly ReleaseSettings releaseSettings;

		public ReleaseEvent (ReleaseSettings releaseSettings) {
			this.releaseSettings = releaseSettings;
		}
	}
}