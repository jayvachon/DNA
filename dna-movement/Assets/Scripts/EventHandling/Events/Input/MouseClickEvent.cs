using UnityEngine;
using System.Collections;

public class MouseClickEvent : GameEvent {
	
	public readonly RaycastHit hit;
	public readonly Transform transform;
	public readonly Vector3 point;
	public readonly bool leftClick;

	public MouseClickEvent (RaycastHit hit, bool leftClick) {
		this.hit = hit;
		this.leftClick = leftClick;
		transform = hit.transform;
		point = hit.point;
	}
}
