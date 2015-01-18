using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	public class PathPoints {

		List<IPathPoint> points = new List<IPathPoint> ();
		public List<IPathPoint> Points {
			get { return points; }
		}

		List<Vector3> positions = new List<Vector3> ();
		public List<Vector3> Positions {
			get { return positions; }
		}

		public IPathPoint LastPoint {
			get {
				if (points.Count == 0) {
					return null;
				}
				return points[points.Count-1]; 
			}
		}

		public int Count {
			get { return points.Count; }
		}

		public bool Empty {
			get { return Count == 0; }
		}

		public bool Add (IPathPoint point) {
			if (point != LastPoint) {
				points.Add (point);
				UpdatePositions ();
				return true;
			} 
			return false;
		}

		void UpdatePositions () {
			positions.Clear ();
			foreach (IPathPoint point in points) {
				positions.Add (point.Position);
			}
		}
	}
}