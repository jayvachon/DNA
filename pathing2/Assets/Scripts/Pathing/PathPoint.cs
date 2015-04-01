using UnityEngine;
using System.Collections;
using GameInput;
using Units;

namespace Pathing {

	public class PathPoint : MBRefs, IPoolable, IDraggable, IClickable {

		public StaticUnitTransform unit;
		public StaticUnitTransform StaticUnitTransform {
			get { return unit; }
			set { unit = value; }
		}

		public void OnDragEnter (DragSettings dragSettings) {
			PathManager.instance.EnterPathPoint (dragSettings, this);
		}

		public void OnDragExit (DragSettings dragSettings) {
			PathManager.instance.ExitPathPoint (dragSettings, this);
		}

		public void OnClick (ClickSettings clickSettings) {}
		public void OnDrag (DragSettings dragSettings) {}
		public void OnCreate () {}
		public void OnDestroy () {}
	}
}