using UnityEngine;
using System.Collections;

public class FigureEight : MonoBehaviour {

	public Transform a;
	public Transform b;

	float t = 0f;
	float distance;
	float TWO_PI;
	float xMax = 1.5f;
	Vector3 prevPosition;

	void Awake () {
		TWO_PI = Mathf.PI * 2f;
		SetDistance ();
		prevPosition = transform.position;
	}

	void Update () {

		t += Time.deltaTime * 0.25f;
		if (t >= 1f) t = 0f;
		transform.localPosition = new Vector3 (GetX (t), 0, GetZ (t));

		Vector3 a = transform.localPosition - prevPosition;
		float angle = Vector3.Angle (a, Vector3.right);
		transform.SetLocalEulerAnglesY (angle + 90f);

		SetDistance ();
		prevPosition = transform.localPosition;
	}

	void SetDistance () {
		distance = Vector3.Distance (a.position, b.position);
	}

	float GetZ (float p) {
		return xMax * Mathf.Sin (TWO_PI * p * 2f);
	}

	float GetX (float p) {
		return (distance + xMax * 2) * Mathf.Sin (TWO_PI * p) / 2f;
	}
}
