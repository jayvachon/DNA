using UnityEngine;
using System.Collections;

public class SelectSelectableEvent : GameEvent {
	
	public Selectable selectable;
	
	public SelectSelectableEvent (Selectable _selectable) {
		selectable = _selectable;
	}
}