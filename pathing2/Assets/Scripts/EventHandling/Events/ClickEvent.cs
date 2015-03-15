using UnityEngine;
using System.Collections;
using GameInput;

namespace GameEvents {
	
	public class ClickEvent : GameEvent {
		
		public readonly ClickSettings clickSettings;

		public ClickEvent (ClickSettings clickSettings) {
			this.clickSettings = clickSettings;
		}
	}
}