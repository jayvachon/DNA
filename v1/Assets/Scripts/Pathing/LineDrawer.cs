using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	[RequireComponent (typeof (LineRenderer))]
	public class LineDrawer : MonoBehaviour {

		LineRenderer lineRenderer;

		public static LineDrawer Create (Transform parent, float startWidth, float endWidth) {
			GameObject go = new GameObject ("LineDrawer", typeof (LineDrawer));
			go.transform.SetParent (parent);
			LineDrawer lineDrawer = go.GetScript<LineDrawer> ();
			lineDrawer.Init (startWidth, endWidth);
			return lineDrawer;
		}

		void Init (float startWidth, float endWidth) {
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