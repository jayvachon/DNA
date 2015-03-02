using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace Pathing {

	public class PathDrawer : MonoBehaviour {
		
		public Path path;
		public LineDrawer pointsDrawer;
		public LineDrawer mouseDrawer;
		
		PathPoints pathPoints = null;
		public PathPoints Points {
			get {
				if (pathPoints == null) {
					pathPoints = path.Points;
				}
				return pathPoints;
			}
		}

		bool dragging = false;
		public bool Dragging {
			get { return dragging; }
			set {
				if (value && !dragging) {
					dragging = true;
					Drag ();
				}
				dragging = value;
			}
		}

		public void OnUpdatePoints () {
			pointsDrawer.UpdatePositions (Points.Positions);
		}

		void Drag () {
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			List<Vector3> positions = new List<Vector3> (new Vector3[2]);
			while (dragging) {
				positions[0] = Points.DragPosition;
				positions[1] = MouseController.MousePosition;
				mouseDrawer.UpdatePositions (positions);
				yield return null;
			}
			mouseDrawer.Clear ();
		}
	}
}