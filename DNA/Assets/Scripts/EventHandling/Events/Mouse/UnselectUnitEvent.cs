using UnityEngine;
using System.Collections;

public class UnselectUnitEvent : GameEvent {
	
	public Unit unit;
	
	public UnselectUnitEvent (Unit _unit) {
		unit = _unit;
	}
}