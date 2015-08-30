using UnityEngine;
using System.Collections;

public class MouseClickEvent : GameEvent {
	
	public RaycastHit hit;
	public Transform transform;
	public Vector3 point;
	public Selectable selectable = null;

	public MouseClickEvent (RaycastHit _hit) {
		hit = _hit;
		transform = hit.transform;
		point = hit.point;
		selectable = transform.GetComponent<Selectable>();
	}
}
