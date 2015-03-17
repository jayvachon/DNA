using UnityEngine;
using System.Collections;
using GameEvents;
using GameInput;
using Units;

public class MainCamera : MonoBehaviour {

	Vector3 target;
	Vector3 Target {
		get { return target; }
		set {
			target = value;
			StartCoroutine (CoMoveToTarget ());
		}
	}

	public Transform center;
	Transform anchor;
	float arrowSpeed = 0.25f;

	void Awake () {
		Events.instance.AddListener<SelectEvent> (OnSelectEvent);
		anchor = transform.parent;
	}

	IEnumerator CoMoveToTarget () {
		
		Vector3 start = anchor.position;
		Vector3 end = target;
		float startZ = transform.localPosition.z;
		float endZ = -10;
	
		float time = 1f; 
		float eTime = 0f;

		while (eTime < time && end == target) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			anchor.position = Vector3.Lerp (start, end, progress);
			transform.SetLocalPositionZ (Mathf.Lerp (startZ, endZ, progress)); 
			yield return null;
		}
	}

	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			center.SetLocalEulerAnglesY (center.localEulerAngles.y + 1);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			center.SetLocalEulerAnglesY (center.localEulerAngles.y - 1);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (transform.localPosition.z < 0)
				transform.SetLocalPositionZ (transform.localPosition.z + 0.25f);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (transform.localPosition.z > -25)
				transform.SetLocalPositionZ (transform.localPosition.z - 0.25f);
		}
		anchor.LookAt (center.position);
	}

	void OnSelectEvent (SelectEvent e) {
		Unit unit = e.unit;
		if (unit != null) {
			Target = unit.Position;
		}
	}
}