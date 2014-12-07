using UnityEngine;
using System.Collections;

public class UnitClickEvent : GameEvent {

	public readonly Unit unit;
	public readonly Transform transform;
	public readonly bool leftClick;

	public UnitClickEvent (Unit unit, bool leftClick) {
		this.unit = unit;
		this.transform = unit.transform;
		this.leftClick = leftClick;
	}
}
