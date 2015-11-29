using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMovementRotation : MonoBehaviour {

	Vector3 ptA;
	Vector3 ptB;
	Vector3 ptC;

	int pointPosition = 0;
	List<Vector3> points;
	List<int> path;

	float pathLength = 5f;
	float angleB = 0f;
	float angleC = 0f;
	float radius = 1f;
	float moverAngle = 0f;

	bool moving = false;
	bool reverse = false;
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
	}

	void DrawRotationLine (Vector3 pivot, Vector3 target) {

		float rad = pathLength * 0.1f;
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
			ptB.x + pathLength * 0.5f * Mathf.Sin (angleC * Mathf.Deg2Rad),
			ptB.y,
			ptB.z + pathLength * 0.5f * Mathf.Cos (angleC * Mathf.Deg2Rad)
		);
	}

	void Update () {
		GizmosDrawer.Instance.Clear ();

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

		float a = ghostRotation.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 v = new Vector3 (
			ghost.x + radius * Mathf.Sin (a),
			ghost.y,
			ghost.z + radius * Mathf.Cos (a)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (ghost, v));
	}

	void StartMover () {
		
		if (moving) return;
		moving = true;

		Vector3 f = GetPointAtPosition (path[0]);
		Vector3 t = GetPointAtPosition (path[1]);
		pointPosition = path[1];

		Quaternion fromAngle = Quaternion.LookRotation (f-t);//Quaternion.LookRotation (t - f);
		Quaternion toAngle;

		path.RemoveAt (0);
		if (path.Count <= 1) {
			path.Clear ();
			toAngle = fromAngle;
		} else {
			toAngle = Quaternion.LookRotation (GetPointAtPosition (path[1]) - GetPointAtPosition (path[0]));
		}

		float speed = 2f;
		float distance = Vector3.Distance (f, t);
		float time = distance / speed;

		Coroutine.Start (
			time, 
			(float p) => { Move (p, f, t, fromAngle, toAngle); }, 
			OnArriveAtPoint
		);
	}

	void Move (float t, Vector3 from, Vector3 to, Quaternion fromAngle, Quaternion toAngle) {

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

		//Quaternion q = Quaternion.Slerp (fromAngle, toAngle, 0.5f);
		//Quaternion q = Quaternion.Slerp (inverse, direction, 0.5f);
		//Quaternion q = Quaternion.Slerp (fromAngle, toAngle, 0.5f) * Quaternion.AngleAxis(180f, Vector3.up);
		Quaternion q = ClockwiseSlerp (fromAngle, toAngle, t);

		Quaternion q2 = Quaternion.Slerp (fromAngle, toAngle, 0.5f);


		//Debug.Log ("------");
		//Debug.Log (fromAngle.eulerAngles.y + ", " + toAngle.eulerAngles.y);
		//Debug.Log (q.eulerAngles.y);

		ghostRotation = Quaternion.Lerp (inverse, q, t);

		if (distanceToOrigin <= 1f) {
			// 0->1
			mover = GetPointAroundAxis (direction, from, direction.eulerAngles.y);

		} else if (distanceToTarget <= 1f) {
			// 1->0
			mover = GetPointAroundAxis (direction, to, q.eulerAngles.y);

		} else {
			mover = ghost;
		}
	}

	void OnArriveAtPoint () {
		moving = false;
		if (path.Count > 0)
			StartMover ();
	}

	Vector3 GetPointAroundAxis (Quaternion direction, Vector3 pivot, float angle) {
		
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
		int t;

		int positionsCount = Points.Count * 2 - 2;
		while (start >= positionsCount-1) {
			start -= positionsCount;
		}

		return start + 1;
	}

	Vector3 GetPointAtPosition (int position) {
		int positionsCount = Points.Count * 2 - 2;
		if (position < Points.Count) {
			return Points[position];
		} else {
			return Points[position - (Points.Count-1)];
		}
	}

	Quaternion ClockwiseSlerp (Quaternion from, Quaternion to, float t) {
		Vector3 a = to * Vector3.forward;
		Vector3 b = from * Vector3.forward;

		float angleA = Mathf.Atan2(a.x, a.z) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2(b.x, b.z) * Mathf.Rad2Deg;
		float angleDiff = Mathf.DeltaAngle (angleA, angleB);
		Debug.Log (angleDiff);
		//Debug.Log (a + " :::::::: " + b);
		return Quaternion.Slerp (from, to, t);
	}
}