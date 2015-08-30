using UnityEngine;
using System.Collections;

public class SpiralLight : MonoBehaviour {

	Light dirLight = null;
	Light DirLight {
		get {
			if (dirLight == null) {
				dirLight = GetComponent<Light> ();
			}
			return dirLight;
		}
	}

	float speed = 0.1f;
	float time = 0f;

	void Update () {
		time += speed * Time.deltaTime;
		DirLight.color = new HSBColor (Mathf.Sin (Mathf.PI * 2f * time * Time.deltaTime), 0.25f, 1f).ToColor ();
	}
}
