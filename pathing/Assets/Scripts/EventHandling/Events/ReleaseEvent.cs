using UnityEngine;
using System.Collections;
using GameInput;

namespace GameEvents {
	
	public class ReleaseEvent : ClickEvent {
		public ReleaseEvent (ClickSettings clickSettings) : base (clickSettings) {}
	}
}