using UnityEngine;
using System.Collections;

namespace GameInput {
	
	public interface IDraggable {
		bool MoveOnDrag { get; }
		Vector3 Position { get; set; }
		void OnDragEnter (DragSettings dragSettings);
		void OnDrag (DragSettings dragSettings);
		void OnDragExit (DragSettings dragSettings);
	}
}