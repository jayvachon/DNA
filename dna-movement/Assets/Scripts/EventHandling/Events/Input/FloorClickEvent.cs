using UnityEngine;
using System.Collections;

public class FloorClickEvent : GameEvent {

	public readonly bool leftClick;

	public FloorClickEvent (bool leftClick) {
		this.leftClick = leftClick;
	}
}
