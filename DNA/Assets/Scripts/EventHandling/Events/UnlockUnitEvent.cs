using UnityEngine;
using System.Collections;

namespace GameEvents {

	public class UnlockUnitEvent : GameEvent {
		
		public readonly string id;

		public UnlockUnitEvent (string id) {
			this.id = id;
		}
	}
}
