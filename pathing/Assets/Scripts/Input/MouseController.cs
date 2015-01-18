using UnityEngine;
using System.Collections;

namespace GameInput {

	public class ClickSettings {
		
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
	}
	
	public class MouseController : MonoBehaviour {

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
				return Camera.main.ScreenToWorldPoint(new Vector3 (mp.x, mp.y, Camera.main.nearClipPlane));
			}
		}

		void FixedUpdate () {

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
				IClickable clickable = t.GetScript<IClickable>();
				if (clickable != null) {
					clickable.Click (new ClickSettings (leftClick, dragging, Input.mousePosition));
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
		}
	}
}