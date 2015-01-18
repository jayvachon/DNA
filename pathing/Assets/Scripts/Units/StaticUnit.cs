using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

public class StaticUnit : Unit, IPathPoint {

	public Vector3 Position {
		get { return transform.position; }
	}

	public override void Click (ClickSettings settings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointClick (this, settings);
		}
	}
}
