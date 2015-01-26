using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

public class StaticUnit : Unit, IPathPoint {

	public Vector3 Position {
		get { return transform.position; }
	}

	public override void Click (bool left) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointClick (this, left);
		}
	}

	public override void Drag (bool left, Vector3 mousePosition) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointDrag (this, left);
		}
	}
}
