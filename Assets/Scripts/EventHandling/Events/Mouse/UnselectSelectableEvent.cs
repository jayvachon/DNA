using UnityEngine;
using System.Collections;

public class UnselectSelectableEvent : GameEvent {
	
	public Selectable selectable;
	
	public UnselectSelectableEvent (Selectable _selectable) {
		selectable = _selectable;
	}
}