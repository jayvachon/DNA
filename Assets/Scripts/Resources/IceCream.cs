using UnityEngine;
using System.Collections;

public class IceCream : MonoBehaviour {

	void Start () {
		ShootUp ();
	}

	void ShootUp () {
		float horSpeed = 0.2f;
		float angle = Random.Range (0f, 360f) * Mathf.Deg2Rad;
		Vector3 direction = new Vector3 (
			Mathf.Cos (angle) * horSpeed,
			1f,
			Mathf.Sin (angle) * horSpeed
		);
		rigidbody.AddForce (direction * 150, ForceMode.Impulse);
	}
}
