using UnityEngine;
using System.Collections;

namespace Pathing {
	
	public static class PathManager {

		static Path activePath;
		public static Path ActivePath {
			set { activePath = value; }
		}

		public static Path AddPoint (IPathPoint point) {
			if (activePath != null) {
				activePath.AddPoint (point);
			}
			return activePath;
		}

		public static Path RemovePoint (IPathPoint point) {
			if (activePath != null) {
				activePath.RemovePoint (point);
			}
			return activePath;
		}
	}
}