using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMover : MonoBehaviour {

	/*bool HasNextPoint {
		get { return pathPosition+1 <= Points.Count-1; }
	}

	List<Vector3> Points {
		get { return test.points; }
	}

	public PathMovementTest test;

	int pathPosition = 0;
	float speed = 1.5f;
	bool moving = false;

	void StartMoving () {
		if (moving) return;
		if (HasNextPoint) {
			MoveToNextPoint ();
		}
	}

	void MoveToNextPoint () {
		Vector3 from = Points[pathPosition];
		Vector3 to = Points[pathPosition+1];
		float distance = Vector3.Distance (from, to);
		float time = distance / speed;

		StartCoroutine (CoMove ());

	}

	IEnumerator CoMove (float time, Vector3 from, Vector3 to) {
		
		
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			
			yield return null;
		}
	}*/
}
