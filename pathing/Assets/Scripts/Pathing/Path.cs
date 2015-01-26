using UnityEngine;
using System.Collections;
using GameInput;
using GameEvents;

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
			Events.instance.AddListener<ReleaseEvent> (OnReleaseEvent);
			pathPoints = new PathPoints ();
			pathDrawer = PathDrawer.Create (transform, pathPoints);
			mover = Mover.Create (pathable, pathPoints);
			transform.SetParent (mover.transform);
		}

		public void PointClick (IPathPoint point, bool left) {
			if (pathPoints.PointCanStart (point)) {
				clickedPoint = point;
				if (left) {
					AddPoint (point);
				} 
			}
		}

		public void PointDrag (IPathPoint point, bool left) {
			if (clickedPoint == null) return;
			pathDrawer.Dragging = true;
			if (left) {
				AddPoint (point);
			}
			if (!left && mover.CanRemovePoint (point)) {
				RemovePoint (point);
			}
		}

		public void Move () {
			mover.Move ();
		}

		/**
		 *	Private functions
		 */

		void AddPoint (IPathPoint point) {
			if (pathPoints.Add (point)) 
				UpdatePoints ();
		}

		void RemovePoint (IPathPoint point) {
			if (pathPoints.Remove (point))
				UpdatePoints ();
		}

		void UpdatePoints () {
			pathDrawer.OnUpdatePoints ();
		}

		/**
		 * 	 Events
		 */

		void OnReleaseEvent (ReleaseEvent e) {
			clickedPoint = null;
			pathDrawer.Dragging = false;
			pathPoints.RemoveSingle ();
		}
	}
}