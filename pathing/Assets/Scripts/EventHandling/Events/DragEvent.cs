using UnityEngine;
using System.Collections;
using GameInput;

namespace GameEvents {
	
	public class DragEvent : ClickEvent {
		public DragEvent (ClickSettings clickSettings) : base (clickSettings) {}
	}
}