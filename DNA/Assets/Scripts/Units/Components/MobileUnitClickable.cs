#undef DRAG_STYLE
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Units {

	// deprecate this

	// TODO: rename to MobileUnitCollider
	#if DRAG_STYLE
	public class MobileUnitClickable : UnitClickable, IDraggable, IReleasable {
		
		/*public bool MoveOnDrag { get { return true; } }

		bool canDrag = true;
		public bool CanDrag {
			get { return canDrag; }
			set { canDrag = value; }
		}*/

	#else
	public class MobileUnitClickable : UnitClickable {
	#endif

		public bool MoveOnDrag { get { return true; } }

		bool canDrag = true;
		public bool CanDrag {
			get { return canDrag; }
			set { canDrag = value; }
		}

		Vector3 screenPoint;
		Vector3 offset;

		new Collider collider;
		Collider Collider {
			get {
				if (collider == null) {
					collider = GetComponent<Collider> ();
				}
				return collider;
			}
		}

		#if DRAG_STYLE
		public void OnDragEnter (DragSettings dragSettings) {
			if (CanDrag && dragSettings.WasClicked) {
				PathManager.Instance.SelectedPath = MobileUnit.Path;
				MobileUnit.OnDragEnter ();
			}
		}

		public void OnDrag (DragSettings dragSettings) {
			if (CanDrag && dragSettings.WasClicked) {
				Vector3 target = MouseController.MousePositionWorld;
				target.y = 0.5f;
				Unit.Position = target;
			}
		}

		public void OnDragExit (DragSettings dragSettings) {
			if (CanDrag && dragSettings.WasClicked) {
				PathManager.Instance.SelectedPath = null;
				UnitClickable collidingUnit = Colliding ();
				if (collidingUnit != null) {
					MobileUnit.OnDragRelease (collidingUnit.Unit);
				}
			}
		}

		public void OnRelease (ReleaseSettings releaseSettings) {
			MobileUnit.OnRelease ();
		}
		#endif

		public UnitClickable Colliding (int layerMask=Physics.DefaultRaycastLayers) {

			int rayCount = 36;
			float radius = 1.25f;
			float deg = 360f / (float)rayCount;
			RaycastHit hit;

			for (int i = 0; i < rayCount; i ++) {
				float radians = (float)i * deg * Mathf.Deg2Rad;
				Vector3 direction = new Vector3 (
					Mathf.Sin (radians) * radius,
					0,
					Mathf.Cos (radians) * radius
				);

				// Rays are cast inwards, so this will only work if the collider has been disabled
				// (otherwise it will register a collision with itself)
				if (Physics.Raycast (Position + direction, -direction, out hit, 0.5f, layerMask)) {
					return hit.transform.GetScript<UnitClickable> ();
				}
			}

			return null;
		}
	}
}