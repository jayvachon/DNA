using UnityEngine;
using System.Collections;

public class SelectUnitEvent : GameEvent {

	public Unit unit;

	public SelectUnitEvent (Unit _unit) {
		unit = _unit;
	}
}