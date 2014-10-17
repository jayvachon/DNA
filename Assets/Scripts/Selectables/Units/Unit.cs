using UnityEngine;
using System.Collections;

public class Unit : Selectable {
	
	Vector3 destPosition;
	
	public override void ClickNothing (MouseClickEvent e) {
		if (Selected) {
			StartMove (e.point);
		}
	}

	void StartMove (Vector3 destination) {
		destPosition = destination;
		StartCoroutine (Move ());
	}

	IEnumerator Move () {
		float speed = 0.5f;
		while (Vector3.Distance (rigidbody.position, destPosition) > 1) {
			rigidbody.MovePosition (Vector3.Lerp (rigidbody.position, destPosition, Time.deltaTime * speed));
			yield return null;
		}
	}
}
