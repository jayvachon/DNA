using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	public class MobileUnitClickable : UnitClickable, IDraggable {
		
		Vector3 screenPoint;
		Vector3 offset;

		public void OnDragEnter (DragSettings dragSettings) {
			//MobileUnit.Position = new Vector3 (3, 0.5f, 0);
			if (dragSettings.WasClicked) {
				screenPoint = Camera.main.WorldToScreenPoint (Unit.Position);
				offset = Unit.Position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			}
		}

		public void OnDrag (DragSettings dragSettings) {
			if (dragSettings.WasClicked) {
				Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
				Unit.Position = curPosition;
				//MobileUnit.Position = MouseController.MousePosition;
				//Debug.Log (MouseController.MousePosition);
			}
		}

		public void OnDragExit (DragSettings dragSettings) {
			//Debug.Log ("exit");
			Vector3 dropPosition = MouseController.MousePositionWorld;
			dropPosition.y = 0.5f;
			Unit.Position = dropPosition;
		}
	}
}