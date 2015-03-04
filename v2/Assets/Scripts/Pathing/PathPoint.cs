using UnityEngine;
using System.Collections;

namespace Pathing {

	public class PathPoint : MBRefs, IPathPoint {

		// TODO: StaticUnits should have this as a child instead of using StaticUnitTransform as the path point
		// this is necessary for e.g. when a plot turns into a building - the unit gets switched out but the
		// path point needs to stay where it is so that paths are not disturbed

		public Vector3 Position {
			get { return MyTransform.position; }
			set { MyTransform.position = value; }
		}
	}
}