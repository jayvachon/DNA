using UnityEngine;
using System.Collections;

public class FigureEight : MonoBehaviour {

	public Transform a;
	public Transform b;

	float t = 0f;
	float distance;
	float TWO_PI;
	Vector3 center;
	float xMax = 1.5f;
	Vector3 prevPosition;
	float direction;

	void Awake () {
		TWO_PI = Mathf.PI * 2f;
		//distance = Vector3.Distance (a.position, b.position) / 2f + 1f;
		distance = Vector3.Distance (a.position, b.position);
		center = new Vector3 (
			(a.position.x + b.position.x) / 2,
			(a.position.y + b.position.y) / 2,
			(a.position.z + b.position.z) / 2
		);
		direction = Vector3.Angle (a.position - b.position, Vector3.forward);
		prevPosition = transform.position;
	}

	void Update () {
		t += Time.deltaTime * 0.05f;
		if (t >= 1f) t = 0f;
		transform.localPosition = new Vector3 (GetZ (), 0, GetX ());
		
		float angle = Vector3.Angle(Vector3.left, transform.localPosition);
		Vector3 cross = Vector3.Cross(Vector3.left, transform.localPosition);
		if (cross.y < 0) angle = -angle;
		Debug.Log (angle);

		transform.localEulerAngles = new Vector3 (0, Vector3.Angle (transform.localPosition - prevPosition, Vector3.forward), 0);
		prevPosition = transform.localPosition;
		//float direction = Vector3.Dot (Vector3.Cross ())
	}

	float GetX () {
		return xMax * Mathf.Sin (TWO_PI * t * 2f);
	}

	float GetZ () {
		return (distance + xMax * 2) * Mathf.Sin (TWO_PI * t) / 2f;
	}

	/*float GetZ () {
		return distance * Mathf.Cos (TWO_PI * t) / (1 + Mathf.Pow (Mathf.Sin (TWO_PI * t), 2));
	}

	float GetX () {
		return distance * Mathf.Sin (TWO_PI * t) * Mathf.Cos (TWO_PI * t) / (1 + Mathf.Pow (Mathf.Sin (TWO_PI * t), 2));
	}

	void Lemniscate () {
		// (x²+y²)²-ax²+by²

		// (2cos(2pi*t)/(1+sin^2(2pi*t)),
		// 2sin(2pi*t)cos(2pi*t)/(1+sin^2(2pi*t)))

		// leading coefficient: (d/2+1) << gives a distance of 1 unit around points
	}*/
}
