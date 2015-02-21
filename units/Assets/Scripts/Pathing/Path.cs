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
				if (enabled) {
					Events.instance.AddListener<ReleaseEvent> (OnReleaseEvent);
				} else {
					Events.instance.RemoveListener<ReleaseEvent> (OnReleaseEvent);
				}
			}
		}

		PathPoints pathPoints;
		PathDrawer pathDrawer;
		Mover mover;

		bool dragging = false;
		
		public static Path Create (IPathable pathable, IPathMover mover) {
			GameObject go = new GameObject ("Path", typeof (Path));
			Path path = go.GetScript<Path> ();
			path.Init (pathable, mover);
			return path;
		}

		/**
		 *	Public functions
		 */

		public void Init (IPathable pathable, IPathMover pathMover) {
			pathPoints = new PathPoints ();
			pathDrawer = PathDrawer.Create (transform, pathPoints);
			mover = Mover.Create (pathable, pathMover, pathPoints);
			transform.SetParent (mover.transform);
		}

		public void PointDragEnter (DragSettings dragSettings, IPathPoint point) {
			if (!dragSettings.left) return;
			if (pathPoints.CanDragFromPoint (point)) {
				dragging = true;
			}

			if (dragging) {
				pathPoints.RequestAdd (point);
				UpdatePoints ();
				pathDrawer.Dragging = true;
			}
		}

		public void PointDragExit (DragSettings dragSettings, IPathPoint point) {
			if (!dragSettings.left) return;
			float a = ScreenPositionHandler.PointDirection (MouseController.MousePosition, point.Position);
			
			if (ScreenPositionHandler.AnglesInRange (pathPoints.Direction, a, 15)) {
				pathPoints.RequestRemove (point);
				UpdatePoints ();
			}
		}

		public void Move () {
			mover.Move ();
		}

		/**
		 *	Private functions
		 */

		void UpdatePoints () {
			pathDrawer.OnUpdatePoints ();
		}

		/**
		 * 	 Events
		 */

		void OnReleaseEvent (ReleaseEvent e) {
			dragging = false;
			pathDrawer.Dragging = false;
			pathPoints.OnRelease ();
		}
	}
}