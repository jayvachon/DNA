using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class ClickPointEvent : GameEvent {

		public readonly PointContainer Container;
		public readonly GridPoint Point;

		public ClickPointEvent (PointContainer point) {
			Container = point;
			Point = Container.Point;
		}
	}
}