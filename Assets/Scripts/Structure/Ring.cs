using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	public RingPoint point;
	public int radius = 160;
	public int sideCount = 12;
	public RingPoint[] points = new RingPoint[0];

	Transform myTransform;

	void Awake () {
		myTransform = transform;
	}

	public void Create (bool offset, int sideCount = 12, int radius = 160) {
		this.radius = radius;
		this.sideCount = sideCount;
		points = new RingPoint[sideCount];
		CreateRing (offset);
	}

	void CreateRing (bool offset) {
		Vector3 pos = myTransform.position;
		float deg = 360f / (float)sideCount;
		float off = offset ? deg / 2 : 0f;
		for (int i = 0; i < sideCount; i ++) {
			float angle = (deg * i + off) * Mathf.Deg2Rad;
			Vector3 pointPos = new Vector3 (
				pos.x,
				pos.y + Mathf.Cos (angle) * radius * 0.75f,
				pos.z + Mathf.Sin (angle) * radius
			);
			points[i] = Instantiate (point, pointPos, Quaternion.identity) as RingPoint;
			points[i].transform.parent = myTransform;
		}
	}

	public Vector3[] GetRingPoints () {
		Vector3[] positions = new Vector3[points.Length];
		for (int i = 0; i < positions.Length; i ++) {
			positions[i] = points[i].WorldPosition ();
		}
		return positions;
	}

	public void DrawRing () {

		int pointCount = points.Length;
		if (pointCount == 0)
			return;

		for (int i = 0; i < pointCount - 1; i ++) {
			Debug.DrawLine (points[i].transform.position, points[i + 1].transform.position, Color.red);
		}
		Debug.DrawLine (points[pointCount - 1].transform.position, points[0].transform.position, Color.red);
	}

	/*void Update () {
		DrawRing ();
	}*/
}
