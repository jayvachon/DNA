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
	}

	protected Vector3 startPosition;
	public Vector3 StartPosition {
		get { return startPosition; }
	}

	protected Vector2 V2Position {
		get {
			Vector3 pos = Camera.main.WorldToScreenPoint (MyTransform.position);
			return new Vector2 (pos.x, Screen.height - pos.y);
		}
	}

	protected virtual void Awake () {
		startPosition = MyTransform.position;
	}

	void OnBecameVisible () {
		//renderer.enabled = true;
	}

	void OnBecameInvisible () {
		//renderer.enabled = false;
	}
}
