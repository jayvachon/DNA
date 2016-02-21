using UnityEngine;
using System.Collections;
using DNA.EventSystem;
using DNA.InputSystem;
using DNA.Units;

public class MainCamera : MBRefs {

	Camera camera = null;
	public Camera Camera {
		get {
			if (camera == null) {
				camera = GetComponent<Camera> ();
			}
			return camera;
		}
	}

	public Transform center;
	Transform anchor;

	float[] zConstraints = new [] { -80f, -5f };
	float[] zoomConstraints = new[] { 15f, 80f };

	void Awake () {
		anchor = transform.parent;
	}

	void Update () {
		
		transform.SetLocalPositionY (
			Mathf.Clamp (transform.localPosition.y + Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * 50,
				zoomConstraints[0],
				zoomConstraints[1])
		);

		center.SetLocalEulerAnglesY (center.localEulerAngles.y - Input.GetAxis ("Horizontal"));
		transform.SetLocalPositionZ (
			Mathf.Clamp (transform.localPosition.z + Input.GetAxis ("Vertical") * 0.5f, 
				zConstraints[0], 
				zConstraints[1]));

		float yLook = Mathf.Lerp (0f, zConstraints[0] / 2f, Mathf.InverseLerp (zConstraints[1], zConstraints[0], transform.localPosition.z));
		Vector3 look = center.position;
		look.y = yLook;
		transform.LookAt (look);
	}
}