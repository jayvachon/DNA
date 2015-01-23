using UnityEngine;
using System.Collections;
using GameInput;

namespace Pathing {

	public class Path : MonoBehaviour {

		new bool enabled = false;
		public bool Enabled {
			get { return enabled; }
			set {
				enabled = value;
			}
		}

		PathPoints pathPoints;
		PathDrawer pathDrawer;
		Mover mover;

		bool dragging = false;
		IPathPoint clickedPoint = null;
		
		public static Path Create (IPathable pathable) {
			GameObject go = new GameObject ("Path", typeof (Path));
			Path path = go.GetScript<Path> ();
			path.Init (pathable);
			return path;
		}

		/**
		 *	Public functions
		 */

		public void Init (IPathable pathable) {
			pathPoints = new PathPoints ();
			pathDrawer = PathDrawer.Create (transform, pathPoints);
			mover = Mover.Create (pathable, pathPoints);
			transform.SetParent (mover.transform);
		}

		public void PointClick (IPathPoint point, bool left) {
			if (pathPoints.Empty || point == pathPoints.LastPoint)
				clickedPoint = point;
		}

		public void PointDrag (IPathPoint point, bool left) {
			if (clickedPoint == null) return;
			if (left) {
				AddPoint (point);
			}
			if (!left && mover.CanRemovePoint (point)) {
				RemovePoint (point);
			}
			if (pathPoints.Empty) {
				pathDrawer.Dragging = false;
			} else {
				pathDrawer.Dragging = true;
			}
		}

		public void PointRelease (IPathPoint point, bool left) {
			clickedPoint = null;
			pathDrawer.Dragging = false;
			pathPoints.RemoveSingle ();
		}

		public void Move () {
			mover.Move ();
		}

		/**
		 *	Private functions
		 */

		bool CanAddPoint (IPathPoint point) {
			if (point == clickedPoint) {
				if (pathPoints.Empty || point == pathPoints.LastPoint)
					return true;
			} else {
				return dragging;
			}
			return false;
		}

		bool CanRemovePoint (IPathPoint point) {
			if (point == clickedPoint) {
				if (point == pathPoints.LastPoint)
					return mover.CanRemovePoint (point);
			} else if (dragging) {
				return mover.CanRemovePoint (point);
			}
			return false;
		}

		void AddPoint (IPathPoint point) {
			pathPoints.Add (point);
			UpdatePoints ();
		}

		void RemovePoint (IPathPoint point) {
			pathPoints.Remove (point);
			UpdatePoints ();
		}

		void UpdatePoints () {
			pathDrawer.OnUpdatePoints ();
		}
	}
}