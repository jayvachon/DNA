using UnityEngine;
using System.Collections;

namespace Pathing {

	public class PathComponent : MBRefs {

		Path path = null;
		protected Path Path {
			get {
				if (path == null) {
					path = MyTransform.parent.GetScript<Path> ();
				}
				return path;
			}
		}

		PathPoints pathPoints = null;
		protected PathPoints Points {
			get {
				if (pathPoints == null) {
					pathPoints = Path.Points;
				}
				return pathPoints;
			}
		}

		IPathable pathable = null;
		protected IPathable Pathable {
			get {
				if (pathable == null) {
					pathable = Path.Pathable;
				}
				return pathable;
			}
		}
	}
}