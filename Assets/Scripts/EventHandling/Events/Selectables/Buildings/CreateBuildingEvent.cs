using UnityEngine;
using System.Collections;

public class CreateBuildingEvent : GameEvent {

	public Vector3 position;
	public string type;

	public CreateBuildingEvent (Vector3 position, string type) {
		this.position = position;
		this.type = type;
	}
}
