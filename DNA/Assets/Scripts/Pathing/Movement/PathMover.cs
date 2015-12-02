using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMover : MBRefs {

	Transform ghost;
	PathRotator rotator;

	void OnEnable () {
		// TODO: dynamically create ghost when the mover begins moving, destroy it when the mover stops
		// cache ghost's position when it stops
		ghost = ObjectPool.Instantiate<PathGhost> ().transform;
		rotator = new PathRotator (MyTransform, ghost, pmr.ptA);
	}

	void OnDisable () {
		// ObjectPool.Destroy<PathGhost> (ghost);
		// ghost = null;
	}

	void Move (float t, Vector3 from, Vector3 to) {
		ghost.position = Vector3.Lerp (from, to, t);
		rotator.ApplyPosition (pathPosition);
	}

	IEnumerator CoMove (Vector3 from, Vector3 to) {
		
		float speed = 2f;
		float distance = Vector3.Distance (from, to);
		float time = distance / speed;
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			//ghost.position = Vector3.Lerp (from, to, eTime / time);
			Move (eTime / time, from, to);
			yield return null;
		}

		OnArriveAtPoint ();
	}

	// testing be below here

	List<Vector3> Points {
		get { return new List<Vector3> () { pmr.ptA, pmr.ptB, pmr.ptC }; }
	}

	public PathMovementRotation pmr;
	int pointPosition = 0;
	int pathPosition = 0;
	List<Vector3> points;
	List<int> path;

	bool moving = false;

	void StartMover () {

		if (moving) return;
		moving = true;

		Vector3 f = GetPointAtPosition (path[pathPosition]);
		Vector3 t = GetPointAtPosition (path[pathPosition+1]);
		pointPosition = path[pathPosition+1];
		Vector3 next = pathPosition+2 < path.Count-1 ? GetPointAtPosition (path[pathPosition+2]) : t;

		rotator.InitMovement (f, t, next);

		pathPosition ++;

		float speed = 2f;
		float distance = Vector3.Distance (f, t);
		float time = distance / speed;

		/*Coroutine.Start (
			time, 
			(float p) => { Move (p, f, t); }, 
			OnArriveAtPoint
		);*/

		StartCoroutine (CoMove (f, t));
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.N)) {
			SetSinglePath ();
			StartMover ();
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			SetMultiplePath (4);
			StartMover ();
		}
	}

	void OnArriveAtPoint () {
		moving = false;
		if (pathPosition == path.Count-1) {
			path.Clear ();
			pathPosition = 0;
		} else {
			StartMover ();
		}
	}

	void SetSinglePath () {

		int f = pointPosition;
		int t = GetNextPathPosition (f);
		path = new List<int> () { f, t };
	}

	void SetMultiplePath (int length) {
		path = new List<int> () { pointPosition };
		for (int i = 0; i < length; i ++) {
			path.Add (GetNextPathPosition (pointPosition+i));
		}
	}

	int GetNextPathPosition (int start) {
		int positionsCount = Points.Count * 2 - 2;
		while (start >= positionsCount-1) {
			start -= positionsCount;
		}

		return start + 1;
	}

	Vector3 GetPointAtPosition (int position) {
		if (position < Points.Count) {
			return Points[position];
		} else {
			return Points[position - (Points.Count-1)];
		}
	}
}
