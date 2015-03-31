using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	public class MobileUnitClickable : UnitClickable, IDraggable {
		
		Vector3 screenPoint;
		Vector3 offset;

		Collider collider;
		Collider Collider {
			get {
				if (collider == null) {
					collider = GetComponent<Collider> ();
				}
				return collider;
			}
		}

		MobileUnit mobileUnit;
		MobileUnit MobileUnit {
			get {
				if (mobileUnit == null) {
					mobileUnit = Unit as MobileUnit;
				}
				return mobileUnit;
			}
		}

		public void OnDragEnter (DragSettings dragSettings) {
			if (dragSettings.WasClicked) {
				gameObject.layer = 11;
				Collider.enabled = false;
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
				gameObject.layer = 8;
				Collider.enabled = true;
				MobileUnit.OnDragRelease ();
			}
		}

		void OnTriggerStay (Collider collider) {
			
		}
	}
}