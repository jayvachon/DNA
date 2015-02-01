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

		IPathPoint queuedPoint = null;
		IPathPoint QueuedPoint {
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
			get { return positions[Count-2]; }
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

		public bool CanDragFromPoint (IPathPoint point) {
			return Empty || point == LastPoint;
		}

		public void RequestAdd (IPathPoint point) {
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

		public void RequestRemove (IPathPoint point) {
			if (point == LastPoint) {
				Remove ();
			}
		}

		public void OnRelease () {
			QueuedPoint = null;
		}

		void Add (IPathPoint point) {
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
			foreach (IPathPoint point in points) {
				positions.Add (point.Position);
			}
		}
	}
}