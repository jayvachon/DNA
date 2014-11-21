using UnityEngine;
using System.Collections;

public class RingPoint : MonoBehaviour {

	Transform myTransform;

	void Awake () {
		myTransform = transform;
	}

	public Vector3 WorldPosition () {
		return myTransform.position;
	}
}
