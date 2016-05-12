using UnityEngine;
using System.Collections;

namespace DNA.Units {

	[RequireComponent (typeof (LineRenderer))]
	public class RangeRing : MBRefs {

		LineRenderer lineRenderer = null;
		LineRenderer LineRenderer {
			get {
				if (lineRenderer == null) {
					lineRenderer = GetComponent<LineRenderer> ();
				}
				return lineRenderer;
			}
		}

		public void Show () {
			LineRenderer.enabled = true;
		}

		public void Hide () {
			LineRenderer.enabled = false;
		}

		public void Set (float radius, int resolution) {

			Vector3[] positions = new Vector3[resolution+1];
			float twopi = Mathf.PI * 2f;

			LineRenderer.SetVertexCount (resolution+1);

			for (int i = 0; i < resolution; i ++) {
				float p = (float)i/(float)(resolution-1);
				positions[i] = new Vector3 (radius * Mathf.Sin (twopi * p), 0f, radius * Mathf.Cos (twopi * p));
			}

			positions[positions.Length-1] = positions[0];

			LineRenderer.SetPositions (positions);
		}

		public static RangeRing Create (Transform parent) {
			RangeRing rangeRing = ObjectPool.Instantiate<RangeRing> ();
			rangeRing.transform.SetParent (parent);
			rangeRing.transform.Reset ();
			return rangeRing;
		}
	}
}