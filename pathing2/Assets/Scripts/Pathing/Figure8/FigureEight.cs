using UnityEngine;
using System.Collections;

public class FigureEight : MonoBehaviour {

	public Transform a;
	public Transform b;

	float t = 0f;
	float distance;
	float TWO_PI;
	Vector3 center;

	void Awake () {
		TWO_PI = Mathf.PI * 2f;
		distance = Vector3.Distance (a.position, b.position) / 2 + 1;
		center = new Vector3 (
			(a.position.x + b.position.x) / 2,
			(a.position.y + b.position.y) / 2,
			(a.position.z + b.position.z) / 2
		);
		Debug.Log (center);
	}

	void Update () {
		t += Time.deltaTime * 0.1f;
		if (t >= 1f) t = 0f;
		transform.position = new Vector3 (center.x + GetX (), center.y, center.z + GetZ ());
	}

	float GetZ () {
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
	}


}
