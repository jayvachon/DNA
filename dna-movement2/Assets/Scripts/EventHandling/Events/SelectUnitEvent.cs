using UnityEngine;
using System.Collections;

public class SelectUnitEvent : GameEvent {

	public readonly Unit unit;

	public SelectUnitEvent (Unit unit) {
		this.unit = unit;
	}
}
