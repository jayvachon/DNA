using UnityEngine;
using System.Collections;

public class PathRotation : MonoBehaviour {

	public Transform a;
	public Transform b;

	void Awake () {
		SetPosition ();
		SetRotation ();
	}

	void Update () {
		SetPosition ();
		SetRotation ();
	}

	void SetPosition () {
		transform.position = new Vector3 (
			(a.position.x + b.position.x) / 2,
			(a.position.y + b.position.y) / 2,
			(a.position.z + b.position.z) / 2
		);
	}

	void SetRotation () {
		Vector3 c = a.position - b.position;
		float angle = Vector3.Angle (c, Vector3.right);
		Vector3 cross = Vector3.Cross (c, Vector3.right);
		angle *= -Mathf.Sign (cross.y);
		transform.SetLocalEulerAnglesY (angle);
	}
}
