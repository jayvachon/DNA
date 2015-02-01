using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

public class StaticUnit : Unit, IPathPoint, IDraggable {

	public Vector3 Position {
		get { return MyTransform.position; }
	}

	public override void OnClick (ClickSettings clickSettings) {
		if (!SelectionManager.NoneSelected && !SelectionManager.IsSelected (this)) return;
		if (clickSettings.left) {
			SelectionManager.ToggleSelect (this);
		} else {
			SelectionManager.Unselect ();
		}
	}

	public void OnDragEnter (DragSettings dragSettings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointDragEnter (dragSettings);
		}
	}

	public void OnDrag (DragSettings dragSettings) {}

	public void OnDragExit (DragSettings dragSettings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointDragExit (dragSettings);
		}
	}
}
