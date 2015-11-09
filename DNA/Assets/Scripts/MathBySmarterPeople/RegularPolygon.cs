using UnityEngine;
using System.Collections;

public class RegularPolygon {

	public readonly int SideCount = 5;
	public readonly float Radius = 75;

	public float Apothem {
		get { return Radius * Mathf.Cos (Mathf.PI / (float)SideCount); }
	}

	public float SideLength {
		get { return 2 * Radius * Mathf.Sin (Mathf.PI / (float)SideCount); }
	}

	Vector3[] positions;
	public Vector3[] Positions {
		get {
			if (positions == null) {
				Init ();
			}
			return positions;
		}
	}

	Vector3[] angles;
	public Vector3[] Angles {
		get {
			if (angles == null) {
				Init ();
			}
			return angles;
		}
	}

	public RegularPolygon (int sideCount, float radius) {
		SideCount = sideCount;
		Radius = radius;
	}

	void Init () {

		positions = new Vector3[SideCount];
		angles = new Vector3[SideCount];
		float deg = 360 / (float)SideCount;

		for (int i = 0; i < positions.Length; i ++) {
			float radians = deg * i * Mathf.Deg2Rad;
			positions[i] = new Vector3 (
				Mathf.Sin (radians) * Apothem,
				0,
				Mathf.Cos (radians) * Apothem
			);
			angles[i] = new Vector3 (0, deg * i, 0);
		}
	}

	public void ApplyAngleX (Transform transform, int angleIndex) {
		Vector3 oldAngles = transform.localEulerAngles;
		Vector3 newAngles = Angles[angleIndex];
		transform.localEulerAngles = new Vector3 (
			newAngles.x,
			oldAngles.y,
			oldAngles.z
		);
	}	

	public void ApplyAngleY (Transform transform, int angleIndex) {
		Vector3 oldAngles = transform.localEulerAngles;
		Vector3 newAngles = Angles[angleIndex];
		transform.localEulerAngles = new Vector3 (
			oldAngles.x,
			newAngles.y,
			oldAngles.z
		);
	}

	public void ApplyAngleZ (Transform transform, int angleIndex) {
		Vector3 oldAngles = transform.localEulerAngles;
		Vector3 newAngles = Angles[angleIndex];
		transform.localEulerAngles = new Vector3 (
			oldAngles.x,
			oldAngles.y,
			newAngles.z
		);
	}
}