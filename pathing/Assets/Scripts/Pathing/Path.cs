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
		Handle startHandle;
		Handle endHandle;

		bool dragging = false;
		IPathPoint clickedPoint = null;
		
		public static Path Create (Transform parent) {
			GameObject go = new GameObject ("Path", typeof (Path));
			go.transform.SetParent (parent);
			Path path = go.GetScript<Path> ();
			path.Init ();
			return path;
		}

		public void Init () {
			pathPoints = new PathPoints ();
			pathDrawer = PathDrawer.Create (transform, pathPoints);
			startHandle = Handle.Create (transform);
			endHandle = Handle.Create (transform);			
		}

		public void PointClick (IPathPoint point, ClickSettings settings) {
			if (settings.Right) return;
			if (!settings.Drag) {
				clickedPoint = point;
			} else {
				if (point == clickedPoint) {
					if (pathPoints.Empty) {
						AddPoint (point);
					} else if (point == pathPoints.LastPoint) {
						pathDrawer.OnAddPoint ();
						Drag ();
					}
				} else {
					if (dragging) {
						AddPoint (point);
					}
				}
			}
		}

		void AddPoint (IPathPoint point) {
			if (pathPoints.Add (point)) {
				pathDrawer.OnAddPoint ();
				Drag ();
			}
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
			pathDrawer.Dragging = false;
			dragging = false;
			clickedPoint = null;
		}
	}
}