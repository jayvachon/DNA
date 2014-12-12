using UnityEngine;
using System.Collections;

public class Action {

	public readonly string name;
	Unit unit = null;

	public Action (string name, Unit unit=null) {
		this.name = name;
		this.unit = unit;
	}

	public void SetUnit (Unit unit) {
		if (this.unit == null)
			this.unit = unit;
	}

	public void Perform () {
		unit.OnPerformAction (this);
	}
}
