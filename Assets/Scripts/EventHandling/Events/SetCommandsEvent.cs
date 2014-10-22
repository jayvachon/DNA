using UnityEngine;
using System.Collections;

public class SetCommandsEvent : GameEvent {
	
	public string[] keys;
	public string[] descriptions;
	
	public SetCommandsEvent (string[] keys, string[] descriptions) {
		this.keys = keys;
		this.descriptions = descriptions;
	}
}
