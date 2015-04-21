using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

namespace Units {

	// rename to MobileUnitCollider

	public class MobileUnitClickable : UnitClickable, IDraggable, IReleasable {
			
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

		public override void OnClick (ClickSettings clickSettings) {
			if (!CanSelect) return;
			if (clickSettings.left) {
				SelectionManager.Select (this);
			}
		}

		public void OnDragEnter (DragSettings dragSettings) {
			if (CanDrag && dragSettings.WasClicked) {
				PathManager.Instance.SelectedPath = MobileUnit.Path;
				MobileTransform.StopMovingOnPath ();
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
				UnitClickable collidingUnit = Colliding ().GetScript<UnitClickable> ();
				if (collidingUnit != null) {
					MobileUnit.OnDragRelease (collidingUnit.Unit);
				}
			}
		}

		public void OnRelease (ReleaseSettings releaseSettings) {
			MobileUnit.OnRelease ();
		}

		public Transform Colliding (int layerMask=Physics.DefaultRaycastLayers) {

			int rayCount = 18;
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
					return hit.transform;
				}
			}

			return null;
		}
	}
}