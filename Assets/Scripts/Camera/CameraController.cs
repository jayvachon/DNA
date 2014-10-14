using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform cam;
	private Transform myTransform;

	private float distance = 100f;
	public float Distance {
		get { return distance; }
		set {
			distance = Mathf.Max (0, value);
			cam.SetLocalPositionZ (distance * -1f);
		}
	}

	private float height = 0f;
	public float Height {
		get { return height; }
		set {
			cam.SetPositionY (value);
			height = value;
		}
	}

	private float pitch = 0f;
	public float Pitch {
		get { return pitch; }
		set {
			myTransform.SetLocalEulerAnglesY (value);
			pitch = value;
		}
	}

	private void Awake () {
		myTransform = transform;
		Distance = 50f;
	}

	private void Start () {
		Events.instance.AddListener<ChangeActiveStepEvent>(OnChangeActiveStep);
	}

	private void Update () {
		cam.LookAt (myTransform.position);
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Pitch -= 1f;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Pitch += 1f;
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			Height += 1f;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			Height -= 1f;
		}
		if (Input.GetKey (KeyCode.Z)) {
			Distance += 1f;
		}
		if (Input.GetKey (KeyCode.X)) {
			Distance -= 1f;
		}
	}

	void OnChangeActiveStep (ChangeActiveStepEvent e) {
		myTransform.SetPositionY (e.step.transform.position.y + 1);
	}
}
