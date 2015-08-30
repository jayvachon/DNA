using UnityEngine;
using System.Collections;
using GameInput;

public class VoronoiPoint : MBRefs, IDraggable {

	public bool MoveOnDrag { get { return true; } }
	public void OnDragEnter (DragSettings dragSettings) {}
	public void OnDrag (DragSettings dragSettings) {}
	public void OnDragExit (DragSettings dragSettings) {}
}
