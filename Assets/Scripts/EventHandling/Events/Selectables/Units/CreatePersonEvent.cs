using UnityEngine;
using System.Collections;

public class CreatePersonEvent : GameEvent {

	public Vector3 position;

	public CreatePersonEvent (Vector3 position) {
		this.position = position;
	}
}
