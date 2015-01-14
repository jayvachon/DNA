using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
	
	public static Vector3 NullPosition = new Vector3 (-1, -1, -1);

	public static void SetVertexPositions (this LineRenderer lineRenderer, Vector3[] positions) {
		lineRenderer.SetVertexCount(positions.Length);
		for (int i = 0; i < positions.Length; i ++) {
			lineRenderer.SetPosition (i, positions[i]);
		}
	}

	public static T GetScript<T> (this Transform transform) where T : class {
		return transform.GetComponent(typeof (T)) as T;
	}

	public static T GetScript<T> (this GameObject gameObject) where T : class {
		return gameObject.GetComponent (typeof (T)) as T;
	}
}
