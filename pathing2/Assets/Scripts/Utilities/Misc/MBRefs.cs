using UnityEngine;
using System.Collections;

// rename to MB
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

	public virtual Vector3 Position {
		get { return MyTransform.position; }
		set { MyTransform.position = value; }
	}

	public Vector3 LocalPosition {
		get { return MyTransform.localPosition; }
		set { MyTransform.localPosition = value; }
	}

	public Quaternion LocalRotation {
		get { return MyTransform.localRotation; }
		set { MyTransform.localRotation = value; }
	}

	public Vector3 LocalEulerAngles {
		get { return MyTransform.localEulerAngles; }
		set { MyTransform.localEulerAngles = value; }
	}

	protected virtual void Awake () {
		startPosition = MyTransform.position;
	}
}
