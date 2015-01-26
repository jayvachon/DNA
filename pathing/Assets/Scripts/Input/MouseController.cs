using UnityEngine;
using System.Collections;
using GameEvents;

namespace GameInput {

	public class ClickSettings : System.Object {

		public readonly bool left;
		public readonly IClickable clickable;
		public readonly Vector2 position;

		public ClickSettings (bool left, IClickable clickable, Vector2 position) {
			this.left = left;
			this.clickable = clickable;
			this.position = position;
		}
	}

	public class MouseController : MonoBehaviour {

		class MouseButton {

			IClickable clicked = null;
			IClickable dragged = null;
			Vector2 mousePosition = Vector2.zero;
			Vector2 startDragPosition = Vector2.zero;
			bool left = true;
			bool mouseDown = false;
			bool dragging = false;
			float dragThreshold = 5;

			public MouseButton (bool left) {
				this.left = left;
			}

			public void HandleMouseDown () {
				UpdateMousePosition ();
				if (!mouseDown) {
					UpdateStartDragPosition ();
					RaiseClick ();
					mouseDown = true;
				} else if (!dragging) {
					CheckDrag ();
				} else if (dragging) {
					RaiseDrag ();
				}
			}

			public void HandleMouseUp () {
				if (mouseDown) {
					mouseDown = false;
					dragging = false;
					RaiseRelease ();				
				}
			}

			void UpdateMousePosition () {
				mousePosition = Input.mousePosition;
			}

			void UpdateStartDragPosition () {
				startDragPosition = mousePosition;
			}

			IClickable GetMouseOver () {
				Ray ray = Camera.main.ScreenPointToRay (mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					return hit.transform.GetScript<IClickable>();
				} else {
					return null;
				}
			}
			
			void CheckDrag () {
				if (Vector2.Distance (startDragPosition, mousePosition) > dragThreshold) {
					dragging = true;
				}
			}

			void RaiseClick () {
				clicked = GetMouseOver ();
				if (clicked != null) {
					clicked.Click (left);
					Events.instance.Raise (new ClickEvent (new ClickSettings (left, clicked, mousePosition)));
				}
			}

			void RaiseDrag () {
				Vector2 c = startDragPosition - mousePosition;
				float angle = Mathf.Atan2 (c.y, c.x) * Mathf.Rad2Deg;
				Debug.Log (angle);
				dragged = GetMouseOver ();
				if (dragged != null) {
					dragged.Drag (left, Vector3.zero);
					Events.instance.Raise (new DragEvent (new ClickSettings (left, dragged, mousePosition)));
				}
			}

			void RaiseRelease () {
				if (clicked != null) {
					clicked.Release (left);	
					Events.instance.Raise (new ReleaseEvent (new ClickSettings (left, clicked, mousePosition)));
				}
			}
		}

		public static Vector3 MousePosition {
			get { 
				Vector2 mp = Input.mousePosition;
				return Camera.main.ScreenToWorldPoint (new Vector3 (mp.x, mp.y, Camera.main.nearClipPlane));
			}
		}

		MouseButton leftButton = new MouseButton (true);
		MouseButton rightButton = new MouseButton (false);

		void LateUpdate () {
			if (Input.GetMouseButton (0)) {
				leftButton.HandleMouseDown ();
			}
			if (Input.GetMouseButton (1)) {
				rightButton.HandleMouseDown ();
			}
			if (!Input.GetMouseButton (0)) {
				leftButton.HandleMouseUp ();
			}
			if (!Input.GetMouseButton (1)) {
				rightButton.HandleMouseUp ();
			}
		}
	}
}