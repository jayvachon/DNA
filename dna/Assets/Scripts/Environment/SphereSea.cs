using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereSea : MonoBehaviour {

	int rows = 50;
	int cols = 50;
	float spacing = 3f;
	List<Transform> parts = new List<Transform> ();

	float damping = 0.95f;

	float[] buffer1;
	float[] buffer2;

	bool toggle = false;

	void Awake () {

		int size = rows * cols;

		buffer1 = new float[size];
		buffer2 = new float[size];

		for (int i = 0; i < rows; i ++) {
			for (int j = 0; j < cols; j ++) {
				Vector3 position = new Vector3 (i * spacing, 0, j * spacing);
				parts.Add (ObjectPool.Instantiate<SphereSeaPart> (position).transform);
			}
		}

		ProcessRipple (buffer1, buffer2);
	}

	void Update () {
		if (toggle) {
			ProcessRipple (buffer1, buffer2);
		} else {
			ProcessRipple (buffer2, buffer1);
		}

		for (int i = 0; i < parts.Count; i ++) {
			parts[i].SetLocalPositionY (buffer1[i]);
		}

		toggle = !toggle;

		if (Input.GetKeyDown (KeyCode.Space)) {
			buffer1[1250] = 30;
			buffer1[1251] = 30;
			buffer1[1249] = 30;
			buffer1[1250-cols] = 30;
			buffer1[1250+cols] = 30;
		}
	}

	void ProcessRipple (float[] source, float[] dest) {
		for (int i = cols; i < source.Length-cols; i ++) {
			dest[i] =
				(source[i-1] +
				 source[i+1] +
				 source[i-cols] +
				 source[i+cols]) / 2 - dest[i];
			dest[i] *= damping;
		}
	}
}
