using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMovementRotation : MonoBehaviour {

	Vector3 ptA;
	Vector3 ptB;
	Vector3 ptC;

	int pointPosition = 0;
	int pathPosition = 0;
	List<Vector3> points;
	List<int> path;

	float pathLength = 5f;
	float angleB = 0f;
	float angleC = 0f;
	float radius = 1f;

	bool moving = false;
	Vector3 ghost = Vector3.zero;
	Vector3 mover = Vector3.zero;
	Quaternion ghostRotation;

	List<Vector3> Points {
		get { return new List<Vector3> () { ptA, ptB, ptC }; }
	}

	void Awake () {
		ptA = Vector3.zero;
		UpdateBPosition ();
		UpdateCPosition ();
		mover.z = ghost.z - radius;

		SetClockRotations ();
	}

	void DrawRotationLine (Vector3 pivot, Vector3 target) {

		float rad = radius;
		float a = Quaternion.LookRotation (target - pivot).eulerAngles.y;
		float r = a * Mathf.Deg2Rad;
		
		Vector3 position = new Vector3 (
			pivot.x + rad * Mathf.Sin (r),
			pivot.y,
			pivot.z + rad * Mathf.Cos (r)
		);

		GizmosDrawer.Instance.Add (new GizmoLine (pivot, position));
	}

	void UpdateBPosition () {
		ptB = new Vector3 (
			ptA.x + pathLength * Mathf.Sin (angleB * Mathf.Deg2Rad),
			ptA.y,
			ptA.z + pathLength * Mathf.Cos (angleB * Mathf.Deg2Rad)
		);
	}

	void UpdateCPosition () {
		ptC = new Vector3 (
			ptB.x + pathLength * 0.75f * Mathf.Sin (angleC * Mathf.Deg2Rad),
			ptB.y,
			ptB.z + pathLength * 0.75f * Mathf.Cos (angleC * Mathf.Deg2Rad)
		);
	}

	void Update () {
		GizmosDrawer.Instance.Clear ();

		if (Input.GetKeyDown (KeyCode.P)) {
			SetClockRotations ();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			StartCoroutine (CoMoveClockHand ());
		}

		if (!moving) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				SetSinglePath ();
				StartMover ();
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				SetMultiplePath (16);
				StartMover ();
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				if (angleB < 360)
					angleB += 2;
				else
					angleB = 0;
				UpdateBPosition ();
				UpdateCPosition ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				if (angleB > 0)
					angleB -= 2;
				else
					angleB = 360;
				UpdateBPosition ();
				UpdateCPosition ();
			}

			if (Input.GetKey (KeyCode.UpArrow)) {
				if (angleC < 360)
					angleC += 2;
				else
					angleC = 0;
				UpdateCPosition ();
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				if (angleC > 0)
					angleC -= 2;
				else
					angleC = 360;
				UpdateCPosition ();
			}
		}

		GizmosDrawer.Instance.Add (new GizmoSphere (ptA, 0.1f));
		GizmosDrawer.Instance.Add (new GizmoSphere (ptB, 0.1f));
		GizmosDrawer.Instance.Add (new GizmoSphere (ptC, 0.1f));
		DrawRotationLine (ptA, ptB);
		DrawRotationLine (ptB, ptA);
		DrawRotationLine (ptB, ptC);
		DrawRotationLine (ptC, ptB);
		GizmosDrawer.Instance.Add (new GizmoSphere (ghost, 0.2f));
		GizmosDrawer.Instance.Add (new GizmoSphere (mover, 0.25f));

		/*float a = ghostRotation.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 v = new Vector3 (
			ghost.x + radius * Mathf.Sin (a),
			ghost.y,
			ghost.z + radius * Mathf.Cos (a)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (ghost, v));*/

		DrawClock ();
	}

	void SetMoverPosition () {

		Vector3 curr = GetPointAtPosition(path[pathPosition]);
		Vector3 prev = pathPosition > 0 ? GetPointAtPosition (path[pathPosition-1]) : curr;
		Vector3 next = pathPosition+1 < path.Count-1 ? GetPointAtPosition (path[pathPosition+1]) : curr;
		Vector3 prev2 = pathPosition > 1 ? GetPointAtPosition (path[pathPosition-2]) : prev;
		Vector3 nearest = curr;
		float distanceToNext = Vector3.Distance (ghost, curr);
		float distanceToPrev = Vector3.Distance (ghost, prev);
		float p = 0f;

		Quaternion from;
		Quaternion to;

		if (distanceToNext < distanceToPrev) {
			nearest = curr;
			from = Quaternion.LookRotation (prev - curr);
			to = Quaternion.LookRotation (next - curr);
			if (distanceToNext <= radius) {
				p = (distanceToNext / radius).Map (1f, 0f, 0f, 0.5f);
			} else {
				p = 0f;
			}
		} else {
			nearest = prev;
			from = Quaternion.LookRotation (prev2 - prev);
			to = Quaternion.LookRotation (curr - prev);
			if (distanceToPrev <= radius) {
				p = (distanceToPrev / radius).Map (0f, 1f, 0.5f, 1f);
			} else {
				p = 1f;
			}
		}

		Quaternion q = from.SlerpClockwise (to, p);

		mover = GetPointAroundAxis (nearest, q.eulerAngles.y);
	}

	void StartMover () {
		
		if (moving) return;
		moving = true;

		int idx = pathPosition;
		int nextIdx = pathPosition+1;

		Vector3 f = GetPointAtPosition (path[idx]);
		Vector3 t = GetPointAtPosition (path[nextIdx]);
		pointPosition = path[nextIdx];

		Quaternion fromAngle = Quaternion.LookRotation (f-t);//Quaternion.LookRotation (t - f);
		Quaternion toAngle;
		Quaternion prevAngle = fromAngle;

		if (pathPosition-1 >= 0) {
			prevAngle = Quaternion.LookRotation (GetPointAtPosition (path[idx]) - GetPointAtPosition (path[idx-1]));
		}

		if (pathPosition < path.Count-1) {
			toAngle = Quaternion.LookRotation (GetPointAtPosition (path[nextIdx+1]) - GetPointAtPosition (path[idx+1]));
			pathPosition ++;
		} else {
			path.Clear ();
			toAngle = fromAngle;
			pathPosition = 0;
		}

		float speed = 2f;
		float distance = Vector3.Distance (f, t);
		float time = distance / speed;

		Coroutine.Start (
			time, 
			(float p) => { Move (p, f, t, fromAngle, toAngle, prevAngle); }, 
			OnArriveAtPoint
		);
	}

	void Move (float t, Vector3 from, Vector3 to, Quaternion fromAngle, Quaternion toAngle, Quaternion prevAngle) {

		Quaternion direction = Quaternion.LookRotation (to - from);
		Quaternion inverse = Quaternion.LookRotation (from - to);
		ghost = Vector3.Lerp (from, to, t);

		float distanceToTarget = Mathf.Infinity;
		float distanceToOrigin = Mathf.Infinity;

		if (t > 0.5f) {
			distanceToTarget = Vector3.Distance (ghost, to) / radius * 0.5f;
		} else {
			distanceToOrigin = 0.5f + (Vector3.Distance (ghost, from) / radius * 0.5f);
		}

		Quaternion q = Quaternion.Slerp (fromAngle, toAngle, 0.5f) * Quaternion.AngleAxis(180f, Vector3.up);

		SetMoverPosition ();

		//Quaternion q = Quaternion.Slerp (inverse, direction, 0.5f);
		//Quaternion q = ClockwiseSlerp (fromAngle, toAngle, t);
		//Quaternion q = fromAngle.SlerpClockwise (toAngle, t);

		//ghostRotation = Quaternion.Lerp (inverse, q, t);

		/*Vector3? nearest = null;
		Quaternion current = new Quaternion ();
		Quaternion next = new Quaternion ();

		float p = 0f;
		if (distanceToTarget <= 1f) {
			p = distanceToTarget.Map (1f, 0f, 0f, 0.5f);
			nearest = to;
			//current = inverse;
			//next = direction;
			current = fromAngle;
			next = toAngle;
		} else if (distanceToOrigin <= 1f) {
			p = distanceToOrigin;//distanceToOrigin.Map (0f, 1f, 0f, 1f);
			nearest = from;
			//current = direction;
			//next = inverse;
			current = prevAngle;
			next = fromAngle;

			Debug.Log ("------------");
			Debug.Log (current.eulerAngles.y + ", " + next.eulerAngles.y);
		} 

		if (nearest != null) {
			Quaternion w = current.SlerpClockwise (next, p);
			mover = GetPointAroundAxis ((Vector3)nearest, w.eulerAngles.y);
		}*/

		/*if (distanceToOrigin <= 1f) {
			// 0->1
			//mover = GetPointAroundAxis (direction, from, direction.eulerAngles.y);

		} else if (distanceToTarget <= 1f) {
			// 1->0
			if (distanceToTarget < 0.5f) {
				mover = GetPointAroundAxis (to, q.eulerAngles.y);
			} else {
				mover = GetPointAroundAxis (to, inverse.eulerAngles.y);
			}
		} else {
			mover = ghost;
		}*/
	}

	void OnArriveAtPoint () {
		moving = false;
		if (path.Count > 0)
			StartMover ();
	}

	Vector3 GetPointAroundAxis (Vector3 pivot, float angle) {
		
		//float r = (direction.eulerAngles.y + angle) * Mathf.Deg2Rad;
		float r = angle * Mathf.Deg2Rad;

		Vector3 position = new Vector3 (
			pivot.x + radius * Mathf.Sin (r),
			pivot.y,
			pivot.z + radius * Mathf.Cos (r)
		);

		return position;
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

	#region Clock

	Quaternion rotA;
	Quaternion rotB;
	Quaternion rotC;

	Vector3 clockOrigin = new Vector3 (-5, 0, 5);
	float clockRadius = 2f;

	void SetClockRotations () {
		float a = Random.Range (0, 360);
		float b = Random.Range (0, 360);
		rotA = Quaternion.AngleAxis (a, Vector3.up);
		rotB = Quaternion.AngleAxis (b, Vector3.up);
		rotC = rotA;
	}

	void DrawClock () {
		float angA = rotA.eulerAngles.y * Mathf.Deg2Rad;
		float angB = rotB.eulerAngles.y * Mathf.Deg2Rad;
		float angC = rotC.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 dirA = new Vector3 (
			clockOrigin.x + clockRadius * Mathf.Sin (angA),
			clockOrigin.y,
			clockOrigin.z + clockRadius * Mathf.Cos (angA)
		);
		Vector3 dirB = new Vector3 (
			clockOrigin.x + clockRadius * Mathf.Sin (angB),
			clockOrigin.y,
			clockOrigin.z + clockRadius * Mathf.Cos (angB)
		);
		Vector3 dirC = new Vector3 (
			clockOrigin.x + clockRadius * Mathf.Sin (angC),
			clockOrigin.y,
			clockOrigin.z + clockRadius * Mathf.Cos (angC)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (clockOrigin, dirA));
		GizmosDrawer.Instance.Add (new GizmoLine (clockOrigin, dirB));
		GizmosDrawer.Instance.Add (new GizmoLine (clockOrigin, dirC));
	}

	IEnumerator CoMoveClockHand () {
		
		float time = 1f;
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			rotC = rotA.SlerpClockwise (rotB, eTime / time);
			yield return null;
		}
	}
	#endregion
}