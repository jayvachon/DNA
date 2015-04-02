using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInput {

	public class DragManager {

		List<DragHandler> drags = new List<DragHandler> ();

		public DragManager (int[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				drags.Add (new DragHandler (true, layers[i]));
				drags.Add (new DragHandler (false, layers[i]));
			}
		}

		public void HandleMouseDown (int mouseButton) {
			for (int i = mouseButton; i < drags.Count; i += 2) {
				drags[i].HandleMouseDown ();
			}
		}

		public void HandleMouseUp (int mouseButton) {
			for (int i = mouseButton; i < drags.Count; i += 2) {
				drags[i].HandleMouseUp ();
			}
		}
	}

	public class DragHandler : MouseButtonHandler<IDraggable> {

		Vector2 startDragPosition = Vector2.zero;
		float dragThreshold = 5;

		bool dragging = false;
		public bool Dragging {
			get { return dragging; }
		}

		IDraggable dragged = null;
		public IDraggable Dragged {
			get { return dragged; }
			set {
				if (dragged != value) {
					IDraggable prevDragged = dragged;
					if (prevDragged != null) {
						prevDragged.OnDragExit (DragSettings);
					}
					dragged = value;
					if (dragged != null) {
						dragged.OnDragEnter (DragSettings);
					}
				} else {
					if (dragged != null)
						dragged.OnDrag (DragSettings);
				}
			}
		}

		DragSettings DragSettings {
			get {
				return new DragSettings (left, Moused, Dragged, Direction);
			}
		}

		float Direction {
			get { return ScreenPositionHandler.PointDirection (startDragPosition, MousePosition); }
		}

		public DragHandler (bool left, int layer) : base (left, layer) {}

		protected override void OnDown () {
			startDragPosition = MousePosition;
		}

		protected override void OnHold () {
			if (!dragging) {
				CheckDrag ();
			} else {
				IDraggable over = GetMouseOver ();
				if (over != null) {
					Dragged = over;
				} else if (Dragged != null) {
					Dragged = Dragged;
				}
			}
		}

		protected override void OnUp () {
			dragging = false;
			Dragged = null;
		}

		void CheckDrag () {
			if (Vector2.Distance (startDragPosition, MousePosition) > dragThreshold) {
				dragging = true;
			}
		}
	}

	public class DragSettings : System.Object {

		public readonly bool left;
		public readonly IDraggable firstDragged;
		public readonly IDraggable draggable;
		public readonly float direction;
		public bool WasClicked {
			get { return firstDragged == draggable; }
		}

		public DragSettings (bool left, IDraggable firstDragged, IDraggable draggable, float direction) {
			this.left = left;
			this.firstDragged = firstDragged;
			this.draggable = draggable;
			this.direction = direction;
		}
	}
}