using UnityEngine;
using System.Collections;

public static class TransformExtensions {

	// Position
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
}
