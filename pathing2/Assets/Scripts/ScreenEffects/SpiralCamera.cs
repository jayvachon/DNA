using UnityEngine;
using System.Collections;

public class SpiralCamera : MonoBehaviour {

	Camera cam = null;
	Camera Cam {
		get {
			if (cam == null) {
				cam = GetComponent<Camera> ();
			}
			return cam;
		}
	}

	float speed = 0.05f;
	float time = 0f;

	/*void Update () {
		time += speed * Time.deltaTime;
		Cam.nearClipPlane = Mathf.Abs (Mathf.Sin (Mathf.PI * 2f * time)) * 15f;
	}*/
}
