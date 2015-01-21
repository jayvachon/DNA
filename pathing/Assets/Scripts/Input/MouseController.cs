using UnityEngine;
using System.Collections;

namespace GameInput {

	/*public class ClickSettings {
		
		bool left = true;
		public bool Left {
			get { return left; }
		}

		public bool Right {
			get { return !left; }
		}

		bool drag = false;
		public bool Drag {
			get { return drag; }
		}

		Vector2 position;
		public Vector2 Position {
			get { return position; }
		}

		public ClickSettings (bool left, bool drag, Vector2 position) {
			this.left = left;
			this.drag = drag;
			this.position = position;
		}
	}*/
	
	public class MouseController : MonoBehaviour {

		class MouseButton {

			IClickable clicked = null;
			bool left = true;
			bool mouseDown = false;
			Vector2 mousePosition = new Vector2 (0, 0);
			Vector2 startDragPosition = new Vector2 (0, 0);
			bool dragging = false;
			float dragThreshold = 10;

			public MouseButton (bool left) {
				this.left = left;
			}

			public void HandleMouseDown () {
				UpdateMousePosition ();
				if (!mouseDown) {
					mouseDown = true;
					UpdateClicked ();
					UpdateStartDragPosition ();
					RaiseClick ();
				} else if (!dragging) {
					CheckDrag ();
				} else if (dragging) {
					UpdateClicked ();
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

			void UpdateClicked () {
				Ray ray = Camera.main.ScreenPointToRay (mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					clicked = hit.transform.GetScript<IClickable>();
				} else {
					clicked = null;
				}
			}
			
			void CheckDrag () {
				if (Vector2.Distance (startDragPosition, mousePosition) > dragThreshold) {
					dragging = true;
				}
			}

			void RaiseClick () {
				if (clicked != null) {
					clicked.Click (left);
				}
			}

			void RaiseDrag () {
				if (clicked != null) {
					clicked.Drag (left, new Vector3 (0, 0, 0));
				}
			}

			void RaiseRelease () {
				if (clicked != null) {
					clicked.Release (left);	
				}
			}
		}

		// temp
		public static bool Dragging { get { return false; }}
		public static Vector3 MousePosition { get { return new Vector3 (0,0,0); }}
		// /temp

		IClickable clicked = null;
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

		/*IClickable lastClicked = null;

		bool mouseDown = false;
		Vector2 startMousePosition;
		float dragThreshold = 10;

		static bool dragging = false;
		public static bool Dragging {
			get { return dragging; }
		}

		public static Vector3 MousePosition {
			get { 
				Vector2 mp = Input.mousePosition;
				return Camera.main.ScreenToWorldPoint (new Vector3 (mp.x, mp.y, Camera.main.nearClipPlane));
			}
		}

		void LateUpdate () {
			
			// Single click
			if (Input.GetMouseButtonDown (0)) {
				MouseDown (true);
				HandleStartDrag ();
			}
			if (Input.GetMouseButtonDown (1)) {
				MouseDown (false);
				HandleStartDrag ();
			}

			// Drag
			CheckDrag ();
			if (dragging) {
				if (Input.GetMouseButton (0)) {
					MouseDown (true);
				}
				if (Input.GetMouseButton (1)) {
					MouseDown (false);
				}
			}

			if (!Input.GetMouseButton (0) && !Input.GetMouseButton (1)) {
				mouseDown = false;
				dragging = false;
			}	
		}

		void MouseDown (bool leftClick) {
			Transform t = HandleMouseDown ();
			if (t != null) {
				lastClicked = t.GetScript<IClickable>();
				if (lastClicked != null) {
					lastClicked.Click (new ClickSettings (leftClick, dragging, Input.mousePosition));
				}
			}
		}

		Transform HandleMouseDown () {
			Vector2 mousePosition = Input.mousePosition;
			
			// Ignore if clicking the GUI --- this is temp, replace it with something better eventually
			if (mousePosition.x < 100 && mousePosition.y > Screen.height-200) return null;

			Ray ray = Camera.main.ScreenPointToRay (mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				return hit.transform;
			}
			return null;
		}

		void HandleStartDrag () {
			mouseDown = true;
			startMousePosition = Input.mousePosition;
		}

		void CheckDrag () {
			if (!mouseDown || dragging) return;
			if (Vector2.Distance (startMousePosition, Input.mousePosition) > dragThreshold) {
				dragging = true;
			}
		}*/
	}
}