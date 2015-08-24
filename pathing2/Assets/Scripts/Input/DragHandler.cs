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
		float dragThreshold = 4;
		IDraggable dragged = null;
		IDraggable currentDragged = null;

		bool dragging = false;
		public bool Dragging {
			get { return dragging; }
		}

		DragSettings DragSettings {
			get { return new DragSettings (left, Moused, dragged, Direction); }
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
				
				// 'currentDragged' is always set to whatever the mouse is over
				// its value gets compared to 'dragged'
				currentDragged = GetMouseOver ();

				// Check for mouse enter
				if (dragged == null) {
					if (currentDragged != null) {
						dragged = currentDragged;
						currentDragged.OnDragEnter (DragSettings);
					}
				} else {

					// if the object is moved when dragged, then OnDragExit gets called on mouse up instead
					if (dragged.MoveOnDrag) {
						dragged.Position = new Vector3 (
							MousePositionWorld.x,
							dragged.Position.y,
							MousePositionWorld.z
						);
						Debug.Log (MousePositionWorld);
						dragged.OnDrag (DragSettings);
					} else {

						// if the object is stationary, then check if the mouse is no longer over it--
						// if not, then call OnDragExit 
						if (currentDragged == null) {
							dragged.OnDragExit (DragSettings);
							dragged = null;
						} else {
							dragged.OnDrag (DragSettings);
						}
					}
				}
			}
		}

		protected override void OnUp () {
			dragging = false;
			if (dragged != null) {
				
				// if the object gets moved when dragged, then only call OnDragExit once the mouse is released
				if (dragged.MoveOnDrag) {
					dragged.OnDragExit (DragSettings);
				}
				dragged = null;
			}
		}

		void CheckDrag () {
			if (Vector2.Distance (startDragPosition, MousePosition) > dragThreshold) {
				dragging = true;
			}
		}
	}

	public class DragSettings {

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