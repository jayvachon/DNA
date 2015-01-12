using UnityEngine;
using System.Collections;

public class MBRefs : MonoBehaviour {

	Transform myTransform;
	public Transform MyTransform {
		get { return myTransform; }
		set { myTransform = value; }
	}

	protected Vector3 startPosition;
	public Vector3 StartPosition {
		get { return startPosition; }
	}

	protected virtual void Awake () {
		myTransform = transform;
		startPosition = myTransform.position;
	}
}
