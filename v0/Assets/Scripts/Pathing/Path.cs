using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {
	
	public class Path {

		readonly IPathable pathable = null;
		List<IPathPoint> points = new List<IPathPoint>();

		IPathPoint prevPoint = null;
		IPathPoint currPoint = null;
		int currPointIndex = 0;

		/**
		 *	Properties
		 */

		public IPathPoint CurrentPoint {
			get { return currPoint; }
		}

		public bool IsLoop {
			get { return FirstPoint == LastPoint; }
		}

		public Vector3 PrevPosition {
			get { return prevPoint.Position; }
		}

		public Vector3 CurrPosition {
			get { return currPoint.Position; }
		}

		public IPathPoint FirstPoint {
			get { return points[0]; }
		}

		public IPathPoint LastPoint {
			get { return points[points.Count-1]; }
		}

		public int Length {
			get { return points.Count; }
		}

		public Path (IPathable pathable) {
			this.pathable = pathable;
		}

		/**
		 *	Public functions
		 */

		public Vector3[] GotoPosition () {
			
			if (points.Count < 2)
				return null;

			if (currPoint == null)
				GotoStartPosition ();
			GotoNextPosition ();

			return new Vector3[] { PrevPosition, CurrPosition };
		}

		// This isn't super elegant-- is there a better way?
		public void ChangePoint (IPathPoint original, IPathPoint newPoint) {
			
			List<Path> originalPaths = original.Paths;
			newPoint.Paths = originalPaths;

			for (int i = 0; i < points.Count; i ++) {
				if (points[i] == original) {
					points[i] = newPoint;
				}
			}
			if (currPoint == original) {
				currPoint = newPoint;
			}
			if (prevPoint == original) {
				prevPoint = newPoint;
			}

			pathable.OnUpdatePath ();
		}

		public void AddPoint (IPathPoint point) {
			if (CanAddPoint (point)) {
				points.Add (point);
			}
			pathable.OnUpdatePath ();
		}

		public void RemovePoint (IPathPoint point) {
			
			if (!CanRemovePoint (point))
				return;

			points.Remove (point);

			// Special case if there are three points in the altered path:
			// if the points form a loop, remove the last point (destroy the loop)
			if (points.Count == 3 && IsLoop) {
				points.Remove (LastPoint);
			}
			pathable.OnUpdatePath ();
		}

		public Vector3[] GetPositions () {
			Vector3[] positions = new Vector3[points.Count];
			for (int i = 0; i < points.Count; i ++) {
				positions[i] = points[i].Position;
			}
			return positions;
		}

		/**
		 *	Private functions
		 */

		Vector3 GotoStartPosition () {
			
			if (points.Count <= 0)
				return ExtensionMethods.NullPosition;

			currPoint = FirstPoint;
			currPointIndex = 0;
			return currPoint.Position;
		}

		Vector3 GotoNextPosition () {
			if (points.Count < 2)
				return ExtensionMethods.NullPosition;
			prevPoint = currPoint;
			SetNextPoint ();
			currPoint = points[currPointIndex];
			return currPoint.Position;
		}

		bool PathHasPoint (IPathPoint point) {
			foreach (IPathPoint p in points) {
				if (p == point)
					return true;
			}
			return false;
		}

		bool CanRemovePoint (IPathPoint point) {
			return PathHasPoint (point) && !(point == prevPoint || point == currPoint);
		}

		bool CanAddPoint (IPathPoint point) {
			if (points.Count == 0)
				return true;
			if (point == LastPoint)
				return false;
			return true;
		}

		void SetNextPoint () {
			
			// Go to the next point in the path
			// If it's the last point in the path:
			// a. loop if the last point is the same as the first point, otherwise,
			// b. reverse the direction of the path

			if (currPointIndex+1 > points.Count-1) {
				if (IsLoop) {
					currPointIndex = 1;
				} else {
					points.Reverse ();
					currPointIndex = 1;
				}
			} else {
				currPointIndex ++;
			}
		}

		/**
		 *	Debugging
		 */

		public void Print () {
			Debug.Log ("==========");
			foreach (IPathPoint p in points) {
				Debug.Log (p.Position);
			}
		}

		public void PrintFirstAndLast () {
			Debug.Log ("===========");
			Debug.Log (FirstPoint.Position);
			Debug.Log (LastPoint.Position);
		}
	}
}