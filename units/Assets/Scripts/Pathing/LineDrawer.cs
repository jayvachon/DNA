using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	[RequireComponent (typeof (LineRenderer))]
	public class LineDrawer : MonoBehaviour {

		public float startWidth = 0.5f;
		public float endWidth = 0.5f;
		LineRenderer lineRenderer;

		void Awake () {
			lineRenderer = GetComponent<LineRenderer> ();
			lineRenderer.SetWidth (startWidth, endWidth);
		}

		public void UpdatePositions (List<Vector3> positions) {
			lineRenderer.SetVertexPositions (positions);
			lineRenderer.enabled = true;
		}

		public void Clear () {
			lineRenderer.enabled = false;
		}
	}
}