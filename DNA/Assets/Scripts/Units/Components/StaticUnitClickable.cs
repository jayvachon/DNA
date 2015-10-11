using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Units {

	public class StaticUnitClickable : UnitClickable {
		
		/*public override void OnClick (ClickSettings clickSettings) {
			if (!CanSelect) return;
			if (clickSettings.left) {
				SelectionManager.Select (this);
			} else {
				if (SelectionManager.IsSelected (this)) {
					SelectionManager.Unselect ();
				}
			}
		}*/

		public override void OnPointerDown (PointerEventData e) {
			if (!CanSelect) return;
			if (e.button == PointerEventData.InputButton.Left) {
				SelectionManager.Select (this);
			} else {
				if (SelectionManager.IsSelected (this)) {
					SelectionManager.Unselect ();
				}
			}
		}	
	}
}