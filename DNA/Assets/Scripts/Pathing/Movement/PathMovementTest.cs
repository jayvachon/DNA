using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (LineRenderer))]
public class PathMovementTest : MonoBehaviour {

	public List<Vector3> points;

	LineRenderer lineRenderer = null;
	LineRenderer LineRenderer {
		get {
			if (lineRenderer == null) {
				lineRenderer = GetComponent<LineRenderer> ();
			}
			return lineRenderer;
		}
	}

	void Awake () {
		LineRenderer.SetVertexPositions (points);
		foreach (Vector3 p in points) {
			ObjectPool.Instantiate<PathMovementPoint> ().transform.position = p;
		}
	}
}
