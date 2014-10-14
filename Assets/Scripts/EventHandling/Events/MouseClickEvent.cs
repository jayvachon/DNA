using UnityEngine;
using System.Collections;

public class MouseClickEvent : GameEvent {
	
	public Transform transform;
	
	public MouseClickEvent (Transform _transform) {
		transform = _transform;
	}
}
