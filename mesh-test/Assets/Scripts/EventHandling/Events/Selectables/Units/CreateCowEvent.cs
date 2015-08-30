using UnityEngine;
using System.Collections;

public class CreateCowEvent : GameEvent {

	public Vector3 position;

	public CreateCowEvent (Vector3 position) {
		this.position = position;
	}
}
