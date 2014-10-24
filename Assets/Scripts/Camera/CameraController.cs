using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform cam;
	Transform myTransform;
	Transform selectableTransform;

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
		Distance = 25f;
	}

	void Start () {
		Events.instance.AddListener<ChangeActiveStepEvent>(OnChangeActiveStep);
		Events.instance.AddListener<SelectSelectableEvent>(OnSelectSelectableEvent);
		Events.instance.AddListener<UnselectSelectableEvent>(OnUnselectSelectableEvent);
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

	void OnSelectSelectableEvent (SelectSelectableEvent e) {
		selectableTransform = e.selectable.transform;
		StartCoroutine (FollowTransform ());
	}

	void OnUnselectSelectableEvent (UnselectSelectableEvent e) {
		if (e.selectable.transform == selectableTransform)
			selectableTransform = null;
	}

	IEnumerator FollowTransform () {
		while (selectableTransform != null) {
			myTransform.position = selectableTransform.position;
			yield return null;
		}
	}
}
