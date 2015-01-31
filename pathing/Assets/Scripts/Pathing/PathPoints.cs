using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

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

		public IPathPoint FirstPoint {
			get {
				return points[0];
			}
		}

		public IPathPoint LastPoint {
			get {
				if (Empty) {
					return null;
				}
				return points[Count-1]; 
			}
		}

		public Vector3 LastPosition {
			get { return positions[Count-1]; }
		}

		public Vector3 PreviousPosition {
			get {
				if (Count < 2) {
					return new Vector3 (-1, -1, -1);
				}
				return positions[Count-2];
			}
		}

		public float Direction {
			get { 
				if (PreviousPosition.Equals (new Vector3 (-1, -1, -1)))
					return 0;
				return ScreenPositionHandler.PointDirection (LastPosition, PreviousPosition); 
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

		public bool PointCanStart (IPathPoint point) {
			return (Empty || point == LastPoint);
		}

		bool CanAddPoint (IPathPoint point) {
			if (point == LastPoint) return false;
			return true;
		}

		public bool Add (IPathPoint point) {
			if (CanAddPoint (point)) {
				points.Add (point);
				UpdatePositions ();
				return true;
			} 
			return false;
		}

		bool CanRemovePoint (IPathPoint point) {
			if (point == LastPoint)
				return true;
			else
				return false;
		}

		public bool Remove (IPathPoint point) {
			if (CanRemovePoint (point)) {
				points.RemoveAt (Count-1);
				UpdatePositions ();
				return true;
			}
			return false;
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