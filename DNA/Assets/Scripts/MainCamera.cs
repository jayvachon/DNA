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

	float[] zConstraints = new [] { -80f, -5f };
	float[] zoomConstraints = new[] { 15f, 80f };

	void Awake () {
		Events.instance.AddListener<PointerDownEvent> (OnPointerDownEvent);
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

	void OnPointerDownEvent (PointerDownEvent e) {
		if (e.data.button == PointerEventData.InputButton.Right) {
			StartCoroutine (CoMoveDrag (Input.mousePosition));
		}
	}

	IEnumerator CoMoveDrag (Vector3 start) {
		
		float startRotation = center.localEulerAngles.y;
		float startPosition = LocalPosition.z;

		while (Input.GetMouseButton (1)) {
			Vector3 delta = Camera.ScreenToViewportPoint (Input.mousePosition - start);
			center.SetLocalEulerAnglesY (startRotation + delta.x * 90);
			// TODO: smoothing
			transform.SetLocalPositionZ (startPosition - delta.y * 30);
			yield return null;
		}
	}
}