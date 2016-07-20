using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.EventSystem;
using DNA.InputSystem;
using DNA.Units;

public class MainCamera : MBRefs {

	Camera cam = null;
	public Camera Camera {
		get {
			if (cam == null) {
				cam = GetComponent<Camera> ();
			}
			return cam;
		}
	}

	public Transform center;

	LowPassFilter lpfHorizontal = new LowPassFilter ();
	LowPassFilter lpfVertical = new LowPassFilter ();
	float[] zConstraints = new [] { -80f, -5f };
	float[] zoomConstraints = new[] { 15f, 80f };
	bool dragging = false;

	void Update () {
		
		transform.SetLocalPositionY (
			Mathf.Clamp (transform.localPosition.y + Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * 
				#if UNITY_STANDALONE_OSX
				50
				#elif UNITY_STANDALONE_WIN
				500
				#else
				50
				#endif
				,
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

		if (Input.GetMouseButtonDown (1)) {
			dragging = true;
			StartCoroutine (CoMoveDrag (Input.mousePosition));
		}
		if (dragging && !Input.GetMouseButton (1)) {
			dragging = false;
			lpfHorizontal.Reset ();
			lpfVertical.Reset ();
		}
	}

	IEnumerator CoMoveDrag (Vector3 start) {
		
		float startRotation = center.localEulerAngles.y;
		float startPosition = LocalPosition.z;

		while (dragging) {

			Vector3 delta = Camera.ScreenToViewportPoint (Input.mousePosition - start);

			float vOffset = lpfVertical.InputSignal (delta.x);
			center.SetLocalEulerAnglesY (startRotation + vOffset * 90);

			float hOffset = lpfHorizontal.InputSignal (delta.y);
			transform.SetLocalPositionZ (startPosition - hOffset * 50f);

			yield return null;
		}
	}
}