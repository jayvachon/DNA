using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	// rename to MobileUnitCollider

	public class MobileUnitClickable : UnitClickable, IDraggable, IReleasable {
			
		public bool MoveOnDrag { get { return true; } }

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
			if (clickSettings.left) {
				SelectionManager.Select (this);
			}
		}

		public void OnDragEnter (DragSettings dragSettings) {
			if (dragSettings.WasClicked) {
				//Collider.enabled = false;
			}
		}

		public void OnDrag (DragSettings dragSettings) {
			if (dragSettings.WasClicked) {
				Vector3 target = MouseController.MousePositionWorld;
				target.y = 0.5f;
				Unit.Position = target;
			}
		}

		public void OnDragExit (DragSettings dragSettings) {
			if (dragSettings.WasClicked) {
				UnitClickable collidingUnit = Colliding ().GetScript<UnitClickable> ();
				if (collidingUnit != null) {
					MobileUnit.OnDragRelease (collidingUnit.Unit);
				}
				//Collider.enabled = true;
			}
		}

		public void OnRelease (ReleaseSettings releaseSettings) {
			MobileTransform.StartMovingOnPath ();
		}

		Transform Colliding () {
			
			int rayCount = 12;
			float radius = 1f;
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
				if (Physics.Raycast (Position + direction, -direction, out hit, 0.5f)) {
					return hit.transform;
				}
			}

			return null;
		}
	}
}