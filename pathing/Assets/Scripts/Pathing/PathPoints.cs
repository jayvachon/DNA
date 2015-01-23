using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	[System.Serializable]
	public class PathPoints : System.Object {

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

		public Vector3 LastPosition {
			get { return positions[positions.Count-1]; }
		}

		public IPathPoint FirstPoint {
			get {
				return points[0];
			}
		}

		public int Count {
			get { return points.Count; }
		}

		public bool Empty {
			get { return Count == 0; }
		}

		public bool Loop {
			get { return FirstPoint == LastPoint; }
		}

		bool CanAddPoint (IPathPoint point) {
			if (point == LastPoint) return false;
			return true;
		}

		public void Add (IPathPoint point) {
			if (CanAddPoint (point)) {
				points.Add (point);
				UpdatePositions ();
			} 
		}

		bool CanRemovePoint (IPathPoint point) {
			if (point == LastPoint)
				return true;
			else
				return false;
		}

		public void Remove (IPathPoint point) {
			if (CanRemovePoint (point)) {
				points.RemoveAt (Count-1);
				UpdatePositions ();
			}
		}

		public void RemoveSingle () {
			if (Count == 1) points.Clear ();
		}

		void UpdatePositions () {
			if (Count == 0) return;
			positions.Clear ();
			foreach (IPathPoint point in points) {
				positions.Add (point.Position);
			}
		}
	}
}