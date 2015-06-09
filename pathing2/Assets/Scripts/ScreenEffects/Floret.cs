using UnityEngine;
using System.Collections;

public class Floret : MonoBehaviour {

	LineRenderer lineRenderer = null;
	LineRenderer LineRenderer {
		get {
			if (lineRenderer == null) {
				lineRenderer = GetComponent<LineRenderer> ();
			}
			return lineRenderer;
		}
	}

	Transform pair;
	Transform Pair {
		get { return pair; }
		set { pair = value; }
	}
}
