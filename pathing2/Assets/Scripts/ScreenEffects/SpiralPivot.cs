using UnityEngine;
using System.Collections;

public class SpiralPivot : MonoBehaviour {

	float speed = 25f;

	void Update () {
		transform.Rotate (Vector3.up * speed * Time.deltaTime);
		if (Input.GetKeyDown (KeyCode.A)) {
			if (speed < 100f)
				speed += 5f;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			if (speed > 0f)
				speed -= 5f;
		}
	}
}
