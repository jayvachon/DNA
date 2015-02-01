using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
	
	public static Vector3 NullPosition = new Vector3 (-1, -1, -1);

	public static bool Equals (this Vector3 vector3, Vector3 otherVector3) {
		return vector3.x == otherVector3.x && vector3.y == otherVector3.y && vector3.z == otherVector3.z;
	}

	public static void SetVertexPositions (this LineRenderer lineRenderer, Vector3[] positions) {
		lineRenderer.SetVertexCount(positions.Length);
		for (int i = 0; i < positions.Length; i ++) {
			lineRenderer.SetPosition (i, positions[i]);
		}
	}

	public static void SetVertexPositions (this LineRenderer lineRenderer, List<Vector3> positions) {
		lineRenderer.SetVertexCount(positions.Count);
		for (int i = 0; i < positions.Count; i ++) {
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
