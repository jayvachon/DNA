using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace Pathing {

	public delegate void OnUpdatePoints ();

	[System.Serializable]
	public class PathPoints : System.Object {

		public OnUpdatePoints OnUpdatePoints { get; set; }

		int maxLength = 2;
		bool allowLoop = false;

		List<PathPoint> points = new List<PathPoint> ();
		public List<PathPoint> Points { get { return points; } }

		List<Vector3> positions = new List<Vector3> ();
		public List<Vector3> Positions { get { return positions; } }

		public Vector3 DragPosition {
			get { return LastPosition; }
		}

		public float Direction {
			get { 
				if (Empty) return -1;
				return ScreenPositionHandler.PointDirection (PreviousPosition, DragPosition); 
			}
		}

		public PathPoint FirstPoint {
			get { return points[0]; }
		}

		public PathPoint LastPoint {
			get {
				if (Empty) return null; 
				return points[Count-1]; 
			}
		}

		public Vector3 LastPosition {
			get { 
				if (Empty) return Vector3.zero;
				return positions[Count-1];
			}
		}

		public Vector3 PreviousPosition {
			get { 
				if (Count > 1) {
					return positions[Count-2]; 
				} 
				return positions[0];
			}
		}

		public int Count { get { return points.Count; } }
		public bool Empty { get { return Count == 0; } }
		public bool Loop { get { return FirstPoint == LastPoint; } }

		public PathPoints (int maxLength, bool allowLoop) {
			this.maxLength = maxLength;
			this.allowLoop = allowLoop;
		}

		public void Add (PathPoint point) {
			if (!CanAddPoint (point)) return;
			points.Add (point);
			if (points.Count > maxLength) {
				RemoveFirst ();
			}
			UpdatePositions ();
		}

		bool CanAddPoint (PathPoint point) {
			if (!point.Enabled
				|| Count > 0
				&& !PointsHavePairs (point)) 
				return false;
				
			if (points.Contains (point)) {
				if (!allowLoop || Count <= 2 || points[Count-1] == point)
					return false;
			}
			return true;
		}

		public void RemoveFirst () {
			if (!Empty) RemoveAt (0);
		}

		public void RemoveLast () {
			if (!Empty) RemoveAt (1);
		}

		void RemoveAt (int index) {
			points.RemoveAt (index);
			UpdatePositions ();
		}

		public void Remove (PathPoint point) {
			if (points.Contains (point)) {
				points.Remove (point);
				UpdatePositions ();
			}
		}

		public void OnRelease () {
			if (Count == 1) 
				RemoveFirst ();
		}

		public void Clear () {
			points.Clear ();
			UpdatePositions ();
		}

		public bool Refresh () {
			if (Count < 2) return false;
			if (!PointsHavePairs (points[0])) {
				Clear ();
				return false;
			}
			return true;
		}

		void UpdatePositions () {
			positions.Clear ();
			foreach (PathPoint point in points) {
				positions.Add (point.Position);
			}
			if (OnUpdatePoints != null) OnUpdatePoints ();
		}

		bool PointsHavePairs (PathPoint newPoint) {
			List<PathPoint> tempPoints = new List<PathPoint> ();
			foreach (PathPoint p in points) {
				tempPoints.Add (p);
			}
			tempPoints.Add (newPoint);
			return newPoint.PointsHavePairs (tempPoints);
		}

		public static bool PathsEqual (List<PathPoint> set1, List<PathPoint> set2) {
			if (set1 == null || set2 == null) return false;
			if (set1.Count != set2.Count) return false;
			for (int i = 0; i < set1.Count; i ++) {
				if (!Vector3.Equals (set1[i].Position, set2[i].Position))
					return false;
			}
			return true;
		}
	}
}