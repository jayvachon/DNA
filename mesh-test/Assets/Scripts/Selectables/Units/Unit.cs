using UnityEngine;
using System.Collections;

public class Unit : Selectable {

	StraightMovementPath movementPath;
	bool moving = false;

	public override void OnStart () {
		OnStartChild ();
	}

	/*public override void ClickNothing (MouseClickEvent e) {
		if (Selected) {
			StartMove (e.point);
		}
	}*/

	public override void ClickOther (MouseClickEvent e) {
		if (e.selectable is DestinationPoint) {
			StartMove (e.transform.position);
		}
	}

	public void StartMove (Vector3 pos) {
		if (moving) return;
		movementPath = new StraightMovementPath (MyTransform.position, pos);
		StartCoroutine (Move (movementPath.Path));
	}

	IEnumerator Move (Vector3[] path) {

		moving = true;

		int p = 0;
		int pathLength = path.Length;

		while (p < pathLength-1) {

			yield return StartCoroutine (MoveStep (path[p], path[p+1]));

			p ++;
			yield return null;
		}

		moving = false;
	}

	IEnumerator MoveStep (Vector3 start, Vector3 end) {
		
		float time = 0.5f;
		float eTime = 0f;

		while (eTime < time && moving) {
			eTime += Time.deltaTime;
			MyTransform.position = Vector3.Lerp (start, end, eTime / time);
			yield return null;
		}
	}

	IEnumerator Stationary (Vector3 centerPoint) {
		yield return null;
	}

	public virtual void OnEndMove () {}
	public virtual void OnStartChild () {}
}
