using UnityEngine;
using System.Collections;

public class FlowerCreatorCreatorCreator : MonoBehaviour {

	public Transform flowerCreatorCreator;

	void Awake () {
		CreateFlower ();
	}

	void CreateFlower () {
		int count = 5;
		float radius = 105;
		float deg = 360 / (float)count;
		for (int i = 0; i < count; i ++) {
			float radians = (deg * i + 18) * Mathf.Deg2Rad;
			Transform t = Instantiate (flowerCreatorCreator) as Transform;
			t.position = new Vector3 (
				Mathf.Sin (radians) * radius,
				0,
				Mathf.Cos (radians) * radius
			);
			t.localEulerAngles = new Vector3 (
				t.localEulerAngles.x,
				deg * i + 36,
				t.localEulerAngles.z
			);
			t.SetParent (transform);
		}
	}
}
