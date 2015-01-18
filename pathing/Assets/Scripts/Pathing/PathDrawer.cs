using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace Pathing {

	[RequireComponent (typeof (LineRenderer))]
	public class PathDrawer : MonoBehaviour {
		
		PathPoints pathPoints;
		List<Vector3> positions = new List<Vector3> ();
		LineRenderer lineRenderer;
		bool dragging = false;
		public bool Dragging {
			set {
				if (value) {
					dragging = true;
					Drag ();
				} else {
					dragging = false;
				}
			}
		}

		int LastPosition {
			get { return positions.Count-1; }
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
			lineRenderer = GetComponent<LineRenderer> ();
		}

		public void OnAddPoint () {
			UpdatePositions ();
			UpdateLineRenderer ();
			Drag ();
		}

		void Drag () {
			positions.Add (positions[LastPosition]);
			lineRenderer.SetWidth (0.5f, 0.01f);
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			while (dragging) {
				positions[LastPosition] = MouseController.MousePosition;
				UpdateLineRenderer ();
				yield return null;
			}
			EndDrag ();
		}

		void EndDrag () {
			positions.RemoveAt (LastPosition);
			lineRenderer.SetWidth (0.5f, 0.5f);
			UpdateLineRenderer ();
		}

		/*void Drag () {
			if (dragging) return;
			dragging = true;
			lineRenderer.SetWidth (0.5f, 0.01f);
			StartCoroutine (CoDrag ());
		}

		IEnumerator CoDrag () {
			while (MouseController.Dragging) {
				positions[LastPosition] = MouseController.MousePosition;
				UpdateLineRenderer ();
				yield return null;	
			}
			EndDrag ();
		}

		void EndDrag () {
			dragging = false;
			positions.RemoveAt (LastPosition);
			lineRenderer.SetWidth (0.5f, 0.5f);
			UpdateLineRenderer ();
		}*/

		void UpdatePositions () {
			positions.Clear ();
			foreach (Vector3 position in pathPoints.Positions) {
				positions.Add (position);
			}

			// this last position is the mouse
			//positions.Add (positions[LastPosition]);
		}

		void UpdateLineRenderer () {
			lineRenderer.SetVertexPositions (positions);
		}
	}
}