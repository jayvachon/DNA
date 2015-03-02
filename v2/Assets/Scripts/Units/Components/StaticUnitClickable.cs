using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	public class StaticUnitClickable : UnitClickable, IDraggable {

		StaticUnit staticUnit = null;
		StaticUnit StaticUnit {
			get {
				if (staticUnit == null) {
					staticUnit = Unit as StaticUnit;
				}
				return staticUnit;
			}
		}

		public override void OnClick (ClickSettings clickSettings) {
			if (!SelectionManager.NoneSelected && !SelectionManager.IsSelected (this))
				return;
			base.OnClick (clickSettings);
		}

		public void OnDragEnter (DragSettings dragSettings) {
			StaticUnit.OnDragEnter (dragSettings);
		}

		public void OnDrag (DragSettings dragSettings) {}

		public void OnDragExit (DragSettings dragSettings) {
			StaticUnit.OnDragExit (dragSettings);
		}
	}
}