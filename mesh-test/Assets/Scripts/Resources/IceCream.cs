using UnityEngine;
using System.Collections;

public class IceCream : MonoBehaviour {
		
	Transform myTransform;

	void Awake () {
		myTransform = transform;
		transform.tag = "Selectable";
	}

	void OnEnable () {
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
		GetComponent<Rigidbody>().AddForce (direction * 150, ForceMode.Impulse);
	}

	public void Collect () {
		ObjectPool.Destroy ("IceCream", myTransform);
	}
}
