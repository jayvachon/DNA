using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace Pathing {

	[System.Serializable]
	public class PathPoints : System.Object {

		List<PathPoint> points = new List<PathPoint> ();
		public List<PathPoint> Points {
			get { return points; }
		}

		List<Vector3> positions = new List<Vector3> ();
		public List<Vector3> Positions {
			get { return positions; }
		}

		PathPoint queuedPoint = null;
		PathPoint QueuedPoint {
			get { return queuedPoint; }
			set { queuedPoint = value; }
		}

		public Vector3 DragPosition {
			get {
				if (QueuedPoint != null) {
					return QueuedPoint.Position;
				} else {
					return LastPosition;
				}
			}
		}

		public float Direction {
			get { 
				if (QueuedPoint == null) {
					return ScreenPositionHandler.PointDirection (PreviousPosition, DragPosition); 
				} else {
					return -1;
				}
			}
		}

		public PathPoint FirstPoint {
			get {
				return points[0];
			}
		}

		public PathPoint LastPoint {
			get {
				if (Empty) {
					return null;
				}
				return points[Count-1]; 
			}
		}

		public Vector3 LastPosition {
			get { 
				if (Empty) {
					return Vector3.zero;
				}
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

		public int Count {
			get { return points.Count; }
		}

		public bool Empty {
			get { return Count == 0; }
		}

		public bool Loop {
			get { return FirstPoint == LastPoint; }
		}

		public bool PointCanStart (PathPoint point) {
			return (Empty || point == LastPoint);
		}

		public bool CanDragFromPoint (PathPoint point) {
			return Empty || point == LastPoint;
		}

		public void RequestAdd (PathPoint point) {
			if (Empty && QueuedPoint == null) {
				QueuedPoint = point;
			} else if (QueuedPoint != null) {
				Add (QueuedPoint);
				Add (point);
				QueuedPoint = null;
			} else {
				if (point != LastPoint) {
					Add (point);
				}
			}
		}

		public void RequestRemove (PathPoint point) {
			if (point == LastPoint) {
				Remove ();
			}
		}

		public void OnRelease () {
			QueuedPoint = null;
		}

		void Add (PathPoint point) {
			points.Add (point);
			UpdatePositions ();
		}

		void Remove () {
			points.RemoveAt (Count-1);
			UpdatePositions ();
		}

		void UpdatePositions () {
			if (Count == 0) return;
			positions.Clear ();
			foreach (PathPoint point in points) {
				positions.Add (point.Position);
			}
		}
	}
}