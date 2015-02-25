using UnityEngine;
using System.Collections;

public class CameraAnchor : MonoBehaviour {

	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.SetLocalEulerAnglesY (transform.localEulerAngles.y + 3);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.SetLocalEulerAnglesY (transform.localEulerAngles.y - 3);
		}
	}
}
