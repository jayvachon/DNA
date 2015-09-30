using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Move to DNA.Paths
namespace Pathing {

	[RequireComponent (typeof (LineRenderer))]
	public class LineDrawer : MonoBehaviour {

		public float startWidth = 0.5f;
		public float endWidth = 0.5f;

		LineRenderer lineRenderer = null;
		LineRenderer LineRenderer {
			get {
				if (lineRenderer == null) {
					lineRenderer = GetComponent<LineRenderer> ();
					lineRenderer.SetWidth (startWidth, endWidth);
				}
				return lineRenderer;
			}
		}
		
		public void UpdatePositions (List<Vector3> positions) {
			LineRenderer.SetVertexPositions (positions);
			LineRenderer.enabled = true;
		}

		public void Clear () {
			LineRenderer.enabled = false;
		}
	}
}