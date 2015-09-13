using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.EventSystem;

namespace Pathing {
	
	/**
	 *	Path
	 *		> PathPositioner
	 *		> PathDrawer
	 *			> PointsDrawer
	 *			> MouseDrawer
	 */

	public class Path : MBRefs, IPoolable {

		bool active = true;
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
		public PathRotator pathRotator;
		public PathDrawer pathDrawer;
		
		PathPoints pathPoints;
		public PathPoints Points {
			get { return pathPoints; }
			private set { pathPoints = value; }
		}

		public IPathable Pathable { get; set; }
		public PathPositioner Positioner { get { return pathPositioner; } }

		public float Speed {
			get { return pathPositioner.Speed; }
			set { pathPositioner.Speed = value; }
		}

		PathSettings pathSettings;
		public PathSettings PathSettings {
			get { return pathSettings; }
			set { pathSettings = value; }
		}
		
		public void Init (IPathable pathable, PathSettings pathSettings, PathRotator pathRotator) {
			Pathable = pathable;
			this.pathSettings = pathSettings;
			this.pathRotator = pathRotator;
			Points = new PathPoints (pathSettings.maxLength, pathSettings.allowLoop);
			pathDrawer.Init (Points);
			Speed = pathSettings.MaxSpeed;
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
			if (pathPoints.Refresh ()) {
				pathPositioner.StartMoving ();
				pathRotator.StartMoving ();
			}
		}

		public void StopMoving () {
			pathPositioner.StopMoving ();
		}

		public void DragFromPath () {
			pathDrawer.Dragging = true;
			pathPoints.RemoveLast ();
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

		public static PathPoint CreatePoint (Vector3 position, DNA.Units.StaticUnit staticUnit) {
			PathPoint pathPoint = ObjectCreator.Instance.Create<PathPoint> (position).GetScript<PathPoint> ();
			pathPoint.StaticUnit = staticUnit;
			Pathfinder.Instance.AddPathPoint (pathPoint);
			return pathPoint;
		}

		public void OnPoolCreate () {}
		public void OnPoolDestroy () {}
	}

	public class PathSettings {

		public float MaxSpeed {
			get { return TimerValues.Instance.MaxPathSpeed; }
		}

		public float MinSpeed {
			get { return TimerValues.Instance.MinPathSpeed; }
		}
		
		public readonly int maxLength;
		public readonly bool allowLoop;

		public PathSettings (int maxLength=2, bool allowLoop=false) {
			this.maxLength = maxLength;
			this.allowLoop = allowLoop;
		}
	}
}