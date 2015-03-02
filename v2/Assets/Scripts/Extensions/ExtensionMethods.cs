using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
	
	/**
	 *	Vector3
	 */
	 
	public static Vector3 NullPosition = new Vector3 (-1, -1, -1);

	public static bool Equals (this Vector3 vector3, Vector3 otherVector3) {
		return vector3.x == otherVector3.x && vector3.y == otherVector3.y && vector3.z == otherVector3.z;
	}

	/**
	 *	LineRenderer
	 */

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

	/**
	 *	Transform
	 */

	public static T GetScript<T> (this Transform transform) where T : class {
		return transform.GetComponent(typeof (T)) as T;
	}

	public static T GetScript<T> (this GameObject gameObject) where T : class {
		return gameObject.GetComponent (typeof (T)) as T;
	}

	public static Transform GetFirstParent (this Transform transform) {
		Transform parent = transform.parent;
		if (parent == null) {
			return null;
		}
		while (parent.parent != null) {
			parent = parent.parent;
		}
		return parent;
	}

	// Position
	public static void SetPosition (this Transform transform, Vector3 position) {
		transform.position = position;
	}
	
	public static void SetLocalPosition (this Transform transform, Vector3 position) {
		transform.localPosition = position;
	}

	public static void SetPositionX (this Transform transform, float x) {
		Vector3 p = transform.position;
		p.x = x;
		transform.position = p;
	}

	public static void SetPositionY (this Transform transform, float y) {
		Vector3 p = transform.position;
		p.y = y;
		transform.position = p;
	}

	public static void SetPositionZ (this Transform transform, float z) {
		Vector3 p = transform.position;
		p.z = z;
		transform.position = p;
	}

	public static void SetLocalPositionX (this Transform transform, float x) {
		Vector3 p = transform.localPosition;
		p.x = x;
		transform.localPosition = p;
	}
	
	public static void SetLocalPositionY (this Transform transform, float y) {
		Vector3 p = transform.localPosition;
		p.y = y;
		transform.localPosition = p;
	}
	
	public static void SetLocalPositionZ (this Transform transform, float z) {
		Vector3 p = transform.localPosition;
		p.z = z;
		transform.localPosition = p;
	}

	// Rotation
	public static void SetLocalEulerAnglesX (this Transform transform, float x) {
		Vector3 r = transform.localEulerAngles;
		r.x = x;
		transform.localEulerAngles = r;
	}

	public static void SetLocalEulerAnglesY (this Transform transform, float y) {
		Vector3 r = transform.localEulerAngles;
		r.y = y;
		transform.localEulerAngles = r;
	}

	public static void SetLocalEulerAnglesZ (this Transform transform, float z) {
		Vector3 r = transform.localEulerAngles;
		r.z = z;
		transform.localEulerAngles = r;
	}

	// Scale
	public static void SetLocalScaleY (this Transform transform, float y) {
		Vector3 p = transform.localScale;
		p.y = y;
		transform.localScale = p;
	}

	public static void SetLocalScale (this Transform transform, float scale) {
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}
