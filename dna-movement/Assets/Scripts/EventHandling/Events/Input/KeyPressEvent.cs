using UnityEngine;
using System.Collections;

public class KeyPressEvent : GameEvent {

	public string key;

	public KeyPressEvent (string key) {
		this.key = key;
	}
}
