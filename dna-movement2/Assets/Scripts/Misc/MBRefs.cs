using UnityEngine;
using System.Collections;

public class MBRefs : MonoBehaviour {

	Transform myTransform = null;
	public Transform MyTransform {
		get { 
			if (myTransform == null) {
				myTransform = transform;
			}
			return myTransform; 
		}
		set { myTransform = value; }
	}

	Vector3 startPosition;
	public Vector3 StartPosition {
		get { return startPosition; }
	}

	void Awake () {
		startPosition = myTransform.position;
		OnAwake ();
	}

	public virtual void OnAwake () {}
}
