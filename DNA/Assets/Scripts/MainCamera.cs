using UnityEngine;
using System.Collections;
using DNA.EventSystem;
using DNA.InputSystem;
using DNA.Units;

public class MainCamera : MBRefs {

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

	float[] zConstraints = new [] { -80f, -5f };
	float[] zoomConstraints = new[] { 15f, 80f };

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
		
		transform.SetLocalPositionY (
			Mathf.Clamp (transform.localPosition.y + Input.GetAxis ("Mouse ScrollWheel"),
				zoomConstraints[0],
				zoomConstraints[1])
		);

		center.SetLocalEulerAnglesY (center.localEulerAngles.y - Input.GetAxis ("Horizontal"));
		transform.SetLocalPositionZ (
			Mathf.Clamp (transform.localPosition.z + Input.GetAxis ("Vertical") * 0.5f, zConstraints[0], zConstraints[1]));

		float yLook = Mathf.Lerp (0f, zConstraints[0] / 2f, Mathf.InverseLerp (zConstraints[1], zConstraints[0], transform.localPosition.z));
		Vector3 look = center.position;
		look.y = yLook;
		transform.LookAt (look);
	}

	void OnSelectEvent (SelectEvent e) {
		/*Unit unit = e.unit;
		if (unit != null) {
			Target = unit.Position;
		}*/
	}
}