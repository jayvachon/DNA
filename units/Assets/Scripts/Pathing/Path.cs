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

		public PathPositioner pathPositioner;
		public PathDrawer pathDrawer;
		
		PathPoints pathPoints;
		public PathPoints Points {
			get { return pathPoints; }
			private set { pathPoints = value; }
		}

		public IPathable Pathable { get; set; }

		bool dragging = false;
		
		public void Init (IPathable pathable) {
			Points = new PathPoints ();
			Pathable = pathable;
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
			pathPositioner.Move ();
		}

		void UpdatePoints () {
			pathDrawer.OnUpdatePoints ();
		}

		void OnReleaseEvent (ReleaseEvent e) {
			dragging = false;
			pathDrawer.Dragging = false;
			pathPoints.OnRelease ();
		}
	}
}