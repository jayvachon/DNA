using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace Pathing {

	public class PathDrawer : PathComponent {
		
		public LineDrawer pointsDrawer;
		public LineDrawer mouseDrawer;

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

		public void Init (PathPoints points) {
			points.OnUpdatePoints += OnUpdatePoints;
		}

		void OnUpdatePoints () {
			pointsDrawer.UpdatePositions (Points.Positions);
		}

		void Drag () {
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			List<Vector3> positions = new List<Vector3> (new Vector3[2]);
			while (dragging) {
				Vector3 startPosition = Points.DragPosition;
				if (startPosition == Vector3.zero) { 
					mouseDrawer.Clear ();
					dragging = false;
				} else {
					positions[0] = startPosition;
					positions[1] = MouseController.MousePosition;
					mouseDrawer.UpdatePositions (positions);
				}
				yield return null;
			}
			mouseDrawer.Clear ();
		}
	}
}