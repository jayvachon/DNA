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

		public void PointClick (IPathPoint point, ClickSettings settings) {
			if (!settings.Drag) {
				clickedPoint = point;
			} else {
				if (settings.Left && CanAddPoint (point)) {
					AddPoint (point);
				}
				if (settings.Right && CanRemovePoint (point)) {
					RemovePoint (point);
				}
			}
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
			clickedPoint = null;
			pathDrawer.OnUpdatePoints ();
			Drag ();
		}

		void Drag () {
			if (dragging) return;
			dragging = true;
			pathDrawer.Dragging = true;
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			while (MouseController.Dragging) {
				yield return null;	
			}
			EndDrag ();
		}

		void EndDrag () {
			pathPoints.RemoveSingle ();
			pathDrawer.Dragging = false;
			dragging = false;
			clickedPoint = null;
		}
	}
}