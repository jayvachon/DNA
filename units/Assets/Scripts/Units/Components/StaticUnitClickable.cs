using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	public class StaticUnitClickable : UnitClickable, IDraggable {

		public override void OnClick (ClickSettings clickSettings) {
			if (!SelectionManager.NoneSelected && !SelectionManager.IsSelected (this))
				return;
			base.OnClick (clickSettings);
		}

		public void OnDragEnter (DragSettings dragSettings) {}
		public void OnDrag (DragSettings dragSettings) {}
		public void OnDragExit (DragSettings dragSettings) {}
	}
}