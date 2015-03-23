using UnityEngine;
using System.Collections;

public class FlowerCreator : MonoBehaviour {

	public Transform pentagonFlower;

	void Awake () {
		//Instantiate (pentagonFlower, Vector3.zero, Quaternion.identity);
		CreateFlowers ();
	}

	void CreateFlowers () {
		int count = 5;
		float radius = 9;
		float deg = 360 / (float)count;
		for (int i = 0; i < count; i ++) {
			float radians = deg * i * Mathf.Deg2Rad;
			Transform t = Instantiate (pentagonFlower) as Transform;
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
