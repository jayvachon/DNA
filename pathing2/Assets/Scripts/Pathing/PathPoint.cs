using UnityEngine;
using System.Collections;
using GameInput;
using Units;

namespace Pathing {

	public class PathPoint : MBRefs, IPoolable, IDraggable {

		public bool MoveOnDrag { get { return false; } }

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
		}

		public StaticUnitTransform unit;
		public StaticUnitTransform StaticUnitTransform {
			get { return unit; }
			set { unit = value; }
		}

		public void OnDragEnter (DragSettings dragSettings) {
			PathManager.Instance.EnterPathPoint (dragSettings, this);
		}

		public void OnDragExit (DragSettings dragSettings) {
			PathManager.Instance.ExitPathPoint (dragSettings, this);
		}

		public void OnDrag (DragSettings dragSettings) {}
		public void OnCreate () {}
		public void OnDestroy () {}
	}
}