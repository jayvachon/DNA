using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace Pathing {

	public class PathDrawer : MonoBehaviour {
		
		PathPoints pathPoints;
		LineDrawer pointsDrawer;
		LineDrawer mouseDrawer;

		bool dragging = false;
		public bool Dragging {
			get { return dragging; }
			set {
				if (value && !dragging) {
					dragging = true;
					Drag ();
				}
				if (!value) {
					dragging = false;
				}
			}
		}

		public static PathDrawer Create (Transform parent, PathPoints pathPoints) {
			GameObject go = new GameObject ("PathDrawer", typeof (PathDrawer));
			go.transform.SetParent (parent);
			PathDrawer drawer = go.GetScript<PathDrawer> ();
			drawer.Init (pathPoints);
			return drawer;
		}

		public void Init (PathPoints pathPoints) {
			this.pathPoints = pathPoints;
			pointsDrawer = LineDrawer.Create (transform, 0.5f, 0.5f);
			mouseDrawer = LineDrawer.Create (transform, 0.5f, 0.01f);
		}

		public void OnUpdatePoints () {
			pointsDrawer.UpdatePositions (pathPoints.Positions);
		}

		void Drag () {
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			List<Vector3> positions = new List<Vector3> (new Vector3[2]);
			while (dragging) {
				positions[0] = pathPoints.LastPosition;
				positions[1] = MouseController.MousePosition;
				mouseDrawer.UpdatePositions (positions);
				yield return null;
			}
			mouseDrawer.Clear ();
		}
	}
}