using UnityEngine;
using System.Collections;

public class PathRotation : MonoBehaviour {

	public Transform a;
	public Transform b;

	void Awake () {
		
		transform.position = new Vector3 (
			(a.position.x + b.position.x) / 2,
			(a.position.y + b.position.y) / 2,
			(a.position.z + b.position.z) / 2
		);

		float angle = Vector3.Angle(Vector3.right, b.position);
		Vector3 cross = Vector3.Cross(Vector3.right, b.position);
		if (cross.y < 0) angle = -angle;
		

		transform.localEulerAngles = new Vector3 (
			0, Vector3.Angle (a.position - b.position, angle < 0 ? Vector3.right : Vector3.left), 0
		);
	}
}
