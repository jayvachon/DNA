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
			
			bool MouseOverIgnore {
				get {
					Ray ray = Camera.main.ScreenPointToRay (MousePosition);
					RaycastHit hit;
					return Physics.Raycast (ray, out hit, Mathf.Infinity, MouseController.IgnoreLayers);
				}
			}

			public MouseButtonHandler (bool left) {
				this.left = left;
			}

			public virtual void HandleMouseDown () {
				MousePosition = Input.mousePosition;
				if (MouseOverIgnore) {
					return;
				}
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
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, MouseController.Layer)) {
					return hit.transform.GetScript<T> ();
				} else {
					return null;
				}
			}
		}

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
					return new DragSettings (left, Moused, Dragged, Direction);
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

		class ClickHandler : MouseButtonHandler<IClickable> {

			public ClickHandler (bool left) : base (left) {}

			protected override void OnDown () {
				ClickSettings clickSettings = new ClickSettings (left, Moused, MousePosition);
				if (Moused != null) {
					Moused.OnClick (clickSettings);
				}
				Events.instance.Raise (new ClickEvent (clickSettings));
			}
		}

		class ReleaseHandler : MouseButtonHandler<IReleasable> {

			public ReleaseHandler (bool left) : base (left) {}

			protected override void OnUp () {
				IReleasable released = GetMouseOver ();
				bool clicked = false;
				if (released != null) {
					clicked = (released == Moused);
				}
				ReleaseSettings releaseSettings = new ReleaseSettings (left, clicked);
				if (released != null) {
					released.OnRelease (releaseSettings);
				}
				Events.instance.Raise (new ReleaseEvent (releaseSettings));
			}
		}

		public static Vector3 MousePosition {
			get { return ScreenPositionHandler.ScreenToWorld (Input.mousePosition); }
		}

		public static Vector3 MousePositionWorld {
			get {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					return hit.point;
				}
				return Vector3.zero;
			}
		}

		static int layer = -1;
		public static int Layer {
			set {
				layer = value;
			}
			get {
				if (layer == -1) {
					layer = LayerController.DefaultLayer;
				}
				return layer;
			}
		}

		public static int IgnoreLayers {
			get { return LayerController.IgnoreLayers; }
		}

		ClickHandler leftClick = new ClickHandler (true);
		ClickHandler rightClick = new ClickHandler (false);
		DragHandler leftDrag = new DragHandler (true);
		DragHandler rightDrag = new DragHandler (false);
		ReleaseHandler leftRelease = new ReleaseHandler (true);
		ReleaseHandler rightRelease = new ReleaseHandler (false);

		void LateUpdate () {
			if (Input.GetMouseButton (0)) {
				leftClick.HandleMouseDown ();
				leftDrag.HandleMouseDown ();
				leftRelease.HandleMouseDown ();
			}
			if (Input.GetMouseButton (1)) {
				rightClick.HandleMouseDown ();
				rightDrag.HandleMouseDown ();
				rightRelease.HandleMouseDown ();
			}
			if (!Input.GetMouseButton (0)) {
				leftClick.HandleMouseUp ();
				leftDrag.HandleMouseUp ();
				leftRelease.HandleMouseUp ();
			}
			if (!Input.GetMouseButton (1)) {
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

	public class ReleaseSettings : System.Object {

		public readonly bool left;
		public readonly bool clicked;

		public ReleaseSettings (bool left, bool clicked) {
			this.left = left;
			this.clicked = clicked;
		}
	}
}