using UnityEngine;
using System.Collections;
using GameInput;
using GameEvents;

namespace Pathing {
	
	/**
	 *	Path
	 *		- PathPositioner
	 *		- PathDrawer
	 *			- PointsDrawer
	 *			- MouseDrawer
	 */

	public class Path : MBRefs, IPoolable {

		new bool active = true;
		public bool Active {
			get { return active; }
			set {
				active = value;
				if (!active) {
					OnInactivate ();
				}
			}
		}

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

		public float Speed {
			get { return pathPositioner.Speed; }
			set { pathPositioner.Speed = value; }
		}

		PathSettings pathSettings;
		public PathSettings PathSettings {
			get { return pathSettings; }
			set { pathSettings = value; }
		}
		
		public void Init (IPathable pathable, PathSettings pathSettings) {
			Pathable = pathable;
			this.pathSettings = pathSettings;
			Points = new PathPoints (pathSettings.maxLength, pathSettings.allowLoop);
			Speed = pathSettings.maxSpeed;
		}

		public void PointDragEnter (DragSettings dragSettings, PathPoint point) {
			if (!Active || !dragSettings.left) return;
			if (pathPoints.CanDragFromPoint (point)) {
				dragging = true;
			} else {

				// Clear the path and start over if the path already exists but the player
				// is dragging over a point not on the path (ik this makes no sense, but it works)
				if (!dragging) {
					pathPoints.Clear ();
					dragging = true;
				}
			}

			if (dragging) {
				pathPoints.RequestAdd (point);
				UpdatePoints ();
				pathDrawer.Dragging = true;
			}
		}

		public void PointDragExit (DragSettings dragSettings, PathPoint point) {
			if (!Active || !dragSettings.left) return;
			float a = ScreenPositionHandler.PointDirection (MouseController.MousePosition, point.Position);
			
			if (ScreenPositionHandler.AnglesInRange (pathPoints.Direction, a, 25)) {
				pathPoints.RequestRemove (point);
				UpdatePoints ();
			}
		}

		public void StartMoving () {
			pathPositioner.StartMoving ();
		}

		public void StopMoving () {
			pathPositioner.StopMoving ();
		}

		void UpdatePoints () {
			pathDrawer.OnUpdatePoints ();
		}

		void OnReleaseEvent (ReleaseEvent e) {
			dragging = false;
			pathDrawer.Dragging = false;
			pathPoints.OnRelease ();
		}

		void OnInactivate () {
			dragging = false;
			pathDrawer.Dragging = false;
			StopMoving ();
			pathPoints.Clear ();
			UpdatePoints ();
		}

		public void OnPoolCreate () {}
		public void OnPoolDestroy () {}
	}

	public class PathSettings {

		public readonly float maxSpeed;
		public readonly int maxLength;
		public readonly bool allowLoop;

		public PathSettings (float maxSpeed=10, int maxLength=2, bool allowLoop=false) {
			this.maxSpeed = maxSpeed;
			this.maxLength = maxLength;
			this.allowLoop = allowLoop;
		}
	}
}