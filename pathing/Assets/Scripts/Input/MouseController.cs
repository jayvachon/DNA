using UnityEngine;
using System.Collections;
using GameEvents;

namespace GameInput {

	public class MouseController : MonoBehaviour {

		abstract class MouseButtonHandler<T> where T : class {

			protected readonly bool left = true;
			bool mouseDown = false;
			
			T moused = null;
			protected T Moused {
				get { return moused; }
			}

			Vector2 mousePosition = Vector2.zero;
			protected Vector2 MousePosition {
				get { return mousePosition; }
				set { mousePosition = value; }
			}

			public MouseButtonHandler (bool left) {
				this.left = left;
			}

			public virtual void HandleMouseDown () {
				MousePosition = Input.mousePosition;
				if (!mouseDown) {
					moused = GetMouseOver ();
					OnDown ();
					mouseDown = true;
				} else {
					OnHold ();
				}
			}

			public virtual void HandleMouseUp () {
				if (mouseDown) {
					OnUp ();
					moused = null;
					mouseDown = false;
				}
			}

			protected virtual void OnDown () {}
			protected virtual void OnHold () {}
			protected virtual void OnUp () {}

			protected T GetMouseOver () {
				Ray ray = Camera.main.ScreenPointToRay (MousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					return hit.transform.GetScript<T> ();
				} else {
					return null;
				}
			}
		}

		// TODO: Break this up into ClickHandler, DragHandler, ReleaseHandler
		/*class MouseButton {

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
				ClickSettings clickSettings = new ClickSettings (left, clicked, mousePosition);
				Events.instance.Raise (new ClickEvent (clickSettings));
				if (clicked != null) {
					//clicked.OnClick (clickSettings);
				}
			}

			void RaiseDrag () {
				dragged = GetMouseOver ();
				ClickSettings clickSettings = new ClickSettings (left, dragged, mousePosition);
				//Events.instance.Raise (new DragEvent (clickSettings));
				if (dragged != null) {
					dragged.Drag (clickSettings);
				}
			}

			void RaiseRelease () {
				ClickSettings clickSettings = new ClickSettings (left, clicked, mousePosition);
				Events.instance.Raise (new ReleaseEvent (clickSettings));
				if (clicked != null) {
					//clicked.Release (clickSettings);	
				}
			}
		}*/

		class DragHandler : MouseButtonHandler<IDraggable> {

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
					return new DragSettings (left, Moused == Dragged, Direction);
				}
			}

			float Direction {
				get { return ScreenPositionHandler.PointDirection (startDragPosition, MousePosition); }
			}

			public DragHandler (bool left) : base (left) {}

			protected override void OnDown () {
				startDragPosition = MousePosition;
			}

			protected override void OnHold () {
				if (!dragging) {
					CheckDrag ();
				} else {
					Dragged = GetMouseOver ();
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

		class ClickHandler : MouseButtonHandler<IClickable> {

			public ClickHandler (bool left) : base (left) {}

			protected override void OnDown () {
				if (Moused != null) {
					Moused.OnClick (new ClickSettings (left, Moused, MousePosition));
				}
			}
		}

		class ReleaseHandler : MouseButtonHandler<IReleasable> {

			public ReleaseHandler (bool left) : base (left) {}

			protected override void OnUp () {
				IReleasable released = GetMouseOver ();
				if (released != null) {
					released.OnRelease (new ReleaseSettings (left, released == Moused));
				}
			}
		}

		public static Vector3 MousePosition {
			get { return ScreenPositionHandler.ScreenToWorld (Input.mousePosition); }
		}

		//MouseButton leftButton = new MouseButton (true);
		//MouseButton rightButton = new MouseButton (false);
		DragHandler leftDrag = new DragHandler (true);
		DragHandler rightDrag = new DragHandler (false);
		ClickHandler leftClick = new ClickHandler (true);
		ClickHandler rightClick = new ClickHandler (false);
		ReleaseHandler leftRelease = new ReleaseHandler (true);
		ReleaseHandler rightRelease = new ReleaseHandler (false);

		void LateUpdate () {
			if (Input.GetMouseButton (0)) {
				//leftButton.HandleMouseDown ();
				leftClick.HandleMouseDown ();
				leftDrag.HandleMouseDown ();
				leftRelease.HandleMouseDown ();
			}
			if (Input.GetMouseButton (1)) {
				//rightButton.HandleMouseDown ();
				rightClick.HandleMouseDown ();
				rightDrag.HandleMouseDown ();
				rightRelease.HandleMouseDown ();
			}
			if (!Input.GetMouseButton (0)) {
				//leftButton.HandleMouseUp ();
				leftClick.HandleMouseUp ();
				leftDrag.HandleMouseUp ();
				leftRelease.HandleMouseUp ();
			}
			if (!Input.GetMouseButton (1)) {
				//rightButton.HandleMouseUp ();
				rightClick.HandleMouseUp ();
				rightDrag.HandleMouseUp ();
				rightRelease.HandleMouseUp ();
			}
		}
	}

	public class ClickSettings : System.Object {

		public readonly bool left;
		public readonly IClickable clickable;
		public readonly Vector2 position;

		public ClickSettings (bool left, IClickable clickable, Vector2 position) {
			this.left = left;
			this.clickable = clickable;
			this.position = position;
		}

		// debugging
		public void Print () {
			Debug.Log ("left: " + left);
			Debug.Log ("clickable: " + clickable);
			Debug.Log ("position: " + position);
		}
	}

	public class DragSettings : System.Object {

		public readonly bool left;
		public readonly bool clicked;
		public readonly float direction;

		public DragSettings (bool left, bool clicked, float direction) {
			this.left = left;
			this.clicked = clicked;
			this.direction = direction;
		}

		// debugging
		public void Print () {
			Debug.Log ("left: " + left);
			Debug.Log ("clicked: " + clicked);
			Debug.Log ("direction: " + direction);
		}
	}

	public class ReleaseSettings : System.Object {

		public readonly bool left;
		public readonly bool clicked;

		public ReleaseSettings (bool left, bool clicked) {
			this.left = left;
			this.clicked = clicked;
		}

		// debugging
		public void Print () {
			Debug.Log ("left: " + left);
			Debug.Log ("clicked: " + clicked);
		}
	}
}