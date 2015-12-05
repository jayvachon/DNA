using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMover : MBRefs {

	// Transform ghost;
	PathRotator rotator;
	PathRotator.Trajectory trajectory;
	public Vector3 ghostPosition;

	void OnEnable () {
		// ghost = ObjectPool.Instantiate<PathGhost> ().transform;
		ghostPosition = pmr.ptA;
		rotator = new PathRotator (MyTransform, pmr.ptA);
	}

	void OnDisable () {
		// ObjectPool.Destroy<PathGhost> (ghost);
		// ghost = null;
	}

	IEnumerator CoMove2 () {
		
		float speed = 2f;

		/** Rotate so that the mover is pointing in the direction it will be going **/

		ghostPosition = trajectory.Origin;
		Vector3 start = ghostPosition;
		Vector3 end = trajectory.GhostStart;
		float distance = trajectory.OriginArc;
		float time = Mathf.Abs (distance / speed) / 2f;
		float eTime = 0f;

		// if mover already pointing in the right direction, snap the ghost to the mover (don't do a rotation)		
		if (Vector3.Distance (Position, end) < 0.1f) {
			ghostPosition = end;
		} else {
			
			// otherwise, do the rotation
			while (eTime < time) {
				eTime += Time.deltaTime;
				ghostPosition = Vector3.Lerp (start, end, eTime / time);
				rotator.ApplyPosition (ghostPosition, pathPosition);
				yield return null;
			}
		}

		/** Move along the path **/

		start = trajectory.GhostStart;
		end = trajectory.GhostEnd;
		distance = Vector3.Distance (start, end);
		time = Mathf.Abs (distance / speed);
		eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			ghostPosition = Vector3.Lerp (start, end, eTime / time);
			rotator.ApplyPosition (ghostPosition, pathPosition);
			yield return null;
		}

		// if this is the final stop, snap ghost to the target (don't do a rotation)
		if (trajectory.TargetIsEnd) {
			ghostPosition = trajectory.Target;
			OnArriveAtPoint ();
			yield break;
		}

		/** If continuing onto another point, rotate to point in the direction of the next point **/
		start = trajectory.GhostEnd;
		end = trajectory.Target;
		distance = trajectory.TargetArc;
		time = Mathf.Abs (distance / speed) / 2f;
		eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			ghostPosition = Vector3.Lerp (start, end, eTime / time);
			rotator.ApplyPosition (ghostPosition, pathPosition);
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
		Vector3 next = pathPosition+1 < path.Count-1 ? GetPointAtPosition (path[pathPosition+2]) : f;

		trajectory = rotator.InitMovement (f, t, next, pathPosition+1 == path.Count-1);

		pathPosition ++;

		float speed = 2f;
		float distance = Vector3.Distance (f, t);
		float time = distance / speed;

		StartCoroutine (CoMove2 ());
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.N)) {
			SetSinglePath ();
			StartMover ();
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			SetMultiplePath (3);
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
