using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

public class StaticUnit : Unit, IPathPoint, IDraggable {

	public Vector3 Position {
		get { return transform.position; }
	}

	/*public override void OnClick (ClickSettings clickSettings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointClick (this, clickSettings);
		}
		//clickSettings.Print ();
	}*/

	/*public override void Drag (ClickSettings clickSettings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointDrag (this, clickSettings);
		}
	}*/

	public void OnDragEnter (DragSettings dragSettings) {
		IPathable pathable = SelectionManager.Selected as IPathable;
		if (pathable != null) {
			pathable.Path.PointDragEnter (this, dragSettings);
		}
	}

	public void OnDrag (DragSettings dragSettings) {
		
	}

	public void OnDragExit (DragSettings dragSettings) {
		
	}

	public void OnRelease (ReleaseSettings releaseSettings) {
		//releaseSettings.Print ();
	}
}
