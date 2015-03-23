using UnityEngine;
using System.Collections;

public class PentagonFlower : MonoBehaviour {

	public Transform pentagon;

	void Awake () {
		CreateFlower ();
	}

	void CreateFlower () {
		int count = 10;
		float radius = 3;
		float deg = 360 / (float)count;
		for (int i = 0; i < count; i ++) {
			float radians = deg * i * Mathf.Deg2Rad;
			Transform t = Instantiate (pentagon) as Transform;
			t.position = new Vector3 (
				Mathf.Sin (radians) * radius,
				0,
				Mathf.Cos (radians) * radius
			);
			t.localEulerAngles = new Vector3 (
				t.localEulerAngles.x,
				deg * i,
				t.localEulerAngles.z
			);
			t.SetParent (transform);
		}
	}
}
