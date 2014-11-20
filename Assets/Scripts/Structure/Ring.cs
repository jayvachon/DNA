using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	public RingPoint point;
	public int radius = 160;
	public int sideCount = 12;
//	public Vector3[] points = new Vector3[0];
	public RingPoint[] points = new RingPoint[0];

	Transform myTransform;

	void Awake () {
		myTransform = transform;
	}

	public void Create (int radius = 160, int sideCount = 12) {
		this.radius = radius;
		this.sideCount = sideCount;
//		points = new Vector3[sideCount];
		points = new RingPoint[sideCount];
		CreateRing ();
	}

	void CreateRing () {
		Vector3 pos = myTransform.position;
		float deg = 360f / (float)sideCount;
		for (int i = 0; i < sideCount; i ++) {
			float angle = deg * i * Mathf.Deg2Rad;
			/*points[i] = new Vector3 (
				pos.x,
				pos.y + Mathf.Cos (angle) * radius,
				pos.z + Mathf.Sin (angle) * radius
			);*/
			Vector3 pointPos = new Vector3 (
				pos.x,
				pos.y + Mathf.Cos (angle) * radius,
				pos.z + Mathf.Sin (angle) * radius
			);
			points[i] = Instantiate (point, pointPos, Quaternion.identity) as RingPoint;
			points[i].transform.parent = myTransform;
		}
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

	void Update () {
		DrawRing ();
	}
}
