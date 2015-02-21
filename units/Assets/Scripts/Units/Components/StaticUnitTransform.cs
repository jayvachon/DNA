using UnityEngine;
using System.Collections;
using Pathing;
using GameInput;

namespace Units {

	public class StaticUnitTransform : UnitTransform, IPathPoint {

		public void OnDragEnter (DragSettings dragSettings) {

			// awfulness
			UnitClickable clickable = SelectionManager.Selected as UnitClickable;
			MobileUnitTransform mobileTransform = clickable.unit.unitTransform as MobileUnitTransform;
			IPathable pathable = mobileTransform as IPathable;

			if (pathable != null) {
				pathable.Path.PointDragEnter (dragSettings, this);
			}
		}

		public void OnDragExit (DragSettings dragSettings) {

			// awfulness
			UnitClickable clickable = SelectionManager.Selected as UnitClickable;
			MobileUnitTransform mobileTransform = clickable.unit.unitTransform as MobileUnitTransform;
			IPathable pathable = mobileTransform as IPathable;

			if (pathable != null) {
				pathable.Path.PointDragExit (dragSettings, this);
			}
		}
	}
}