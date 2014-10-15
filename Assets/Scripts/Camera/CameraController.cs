using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform cam;
	Transform myTransform;
	Transform unitTransform;

	float distance = 100f;
	public float Distance {
		get { return distance; }
		set {
			distance = Mathf.Max (0, value);
			cam.SetLocalPositionZ (distance * -1f);
		}
	}

	float height = 0f;
	public float Height {
		get { return height; }
		set {
			cam.SetPositionY (value);
			height = value;
		}
	}

	float pitch = 0f;
	public float Pitch {
		get { return pitch; }
		set {
			myTransform.SetLocalEulerAnglesY (value);
			pitch = value;
		}
	}

	void Awake () {
		myTransform = transform;
		Distance = 50f;
	}

	void Start () {
		Events.instance.AddListener<ChangeActiveStepEvent>(OnChangeActiveStep);
		Events.instance.AddListener<SelectUnitEvent>(OnSelectUnitEvent);
		Events.instance.AddListener<UnselectUnitEvent>(OnUnselectUnitEvent);
	}

	void Update () {
		cam.LookAt (myTransform.position);
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Pitch += 1f;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Pitch -= 1f;
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

	void OnSelectUnitEvent (SelectUnitEvent e) {
		unitTransform = e.unit.transform;
		StartCoroutine (FollowTransform ());
	}

	void OnUnselectUnitEvent (UnselectUnitEvent e) {
		unitTransform = null;
	}

	IEnumerator FollowTransform () {
		while (unitTransform != null) {
			myTransform.position = unitTransform.position;
			yield return null;
		}
	}
}
