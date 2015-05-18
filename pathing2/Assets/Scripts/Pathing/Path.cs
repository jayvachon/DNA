using UnityEngine;
using System.Collections;
using GameInput;
using GameEvents;

namespace Pathing {
	
	/**
	 *	Path
	 *		> PathPositioner
	 *		> PathDrawer
	 *			> PointsDrawer
	 *			> MouseDrawer
	 */

	public class Path : MBRefs, IPoolable {

		new bool active = true;
		public bool Active {
			get { return active; }
			set {
				active = value;
				if (!active) {
					OnDeactivate ();
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
			pathDrawer.Init (Points);
			Speed = pathSettings.maxSpeed;
		}

		public void PointDragEnter (DragSettings dragSettings, PathPoint point) {
			if (!Active || !dragSettings.left) return;
			pathDrawer.Dragging = true;
			pathPoints.Add (point);
		}

		public void PointDragExit (DragSettings dragSettings, PathPoint point) {
			if (!Active || !dragSettings.left) return;
			float a = ScreenPositionHandler.PointDirection (MouseController.MousePosition, point.Position);
			if (ScreenPositionHandler.AnglesInRange (pathPoints.Direction, a, 25)) {
				pathPoints.Remove (point);
			}
		}

		public void StartMoving () {
			if (pathPoints.Refresh ())
				pathPositioner.StartMoving ();
		}

		public void StopMoving () {
			pathPositioner.StopMoving ();
		}

		public void DragFromPath () {
			pathDrawer.Dragging = true;
			pathPoints.RemoveFirst ();
			StopMoving ();
		}

		void OnReleaseEvent (ReleaseEvent e) {
			pathDrawer.Dragging = false;
			pathPoints.OnRelease ();
		}

		void OnDeactivate () {
			pathDrawer.Dragging = false;
			StopMoving ();
			pathPoints.Clear ();
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