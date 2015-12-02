using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMovementRotation : MonoBehaviour {

	public class RotationPair {

		public readonly Vector3 Pivot;
		public readonly Quaternion From;
		public readonly Quaternion To;
		public readonly bool EndPoint;

		Vector3? fromPosition = null;
		public Vector3 FromPosition {
			get {
				if (fromPosition == null) {
					fromPosition = GetPointAroundAxis (Pivot, From.eulerAngles.y);
				}
				return (Vector3)fromPosition;
			}
		}

		Vector3? toPosition = null;
		public Vector3 ToPosition {
			get {
				if (toPosition == null) {
					toPosition = GetPointAroundAxis (Pivot, To.eulerAngles.y);
				}
				return (Vector3)toPosition;
			}
		}

		public RotationPair (Vector3 pivot, Quaternion from, Quaternion to, bool endPoint) {
			Pivot = pivot;
			From = from;
			To = to;
			EndPoint = endPoint;
		}

		public Vector3 GetPointAroundAxis (Vector3 pivot, float angle) {
			
			float radius = 1f;
			float r = angle * Mathf.Deg2Rad;

			Vector3 position = new Vector3 (
				pivot.x + radius * Mathf.Sin (r),
				pivot.y,
				pivot.z + radius * Mathf.Cos (r)
			);

			return position;
		}
	}

	public class Rotator {

		RotationPair targetRot;
		RotationPair originRot;
		RotationPair nearest;
		Vector3 prevPosition;
		float proximity;
		float radius = 1f;

		public Vector3 Position { get; set; }

		// TODO: pass in transform instead & set the start position here
		public void Init (Vector3 startPosition) {
			prevPosition = startPosition;
		}

		public void InitMovement (Vector3 origin, Vector3 target, Vector3 next) {

			Quaternion to = Quaternion.identity;
			Quaternion from = Quaternion.identity;
			
			if (next != target) {
				to = Quaternion.LookRotation (next - target); 		// set "to" rotation to look at the next point
				from = Quaternion.LookRotation (origin - target); 	// set "from" rotation to look at the previous point
			} else if (origin != target) {
				to = Quaternion.LookRotation (origin - target);
				from = to;
			}

			targetRot = new RotationPair (target, from, to, (origin != target && next == target));

			to = Quaternion.LookRotation (target - origin);
			from = to;
			originRot = new RotationPair (origin, from, to, false);
		}

		// TODO: instead of returning a position, apply position to a transform
		public Vector3 GetPosition (Vector3 ghostPosition, int pathPosition) {
			
			bool approachingTarget = OnApproachTarget (ghostPosition);
			bool departingOrigin = OnDepartOrigin (ghostPosition, pathPosition);

			if (!approachingTarget && !departingOrigin) {
				return ghostPosition;
			} else {

				// don't spin if approaching the final target
				if (nearest.EndPoint)
					return nearest.ToPosition;

				Quaternion from = Quaternion.LookRotation (prevPosition - nearest.Pivot);

				// special case that prevents spinning around the point we're starting from if already pointing in the right direction
				if (departingOrigin && pathPosition == 1 && nearest.To == from)
					return nearest.ToPosition;

				Quaternion q = from.SlerpClockwise (nearest.To, proximity);

				return nearest.GetPointAroundAxis (nearest.Pivot, q.eulerAngles.y);
			}
		}

		bool OnApproachTarget (Vector3 ghostPosition) {

			float distance = Vector3.Distance (ghostPosition, targetRot.Pivot);
			if (distance > radius)
				return false;

			proximity = (distance / radius).Map (1f, 0f, 0f, 0.5f);
			nearest = targetRot;
			prevPosition = targetRot.FromPosition;

			return true;
		}

		bool OnDepartOrigin (Vector3 ghostPosition, int pathPosition) {

			float distance = Vector3.Distance (ghostPosition, originRot.Pivot);
			if (distance > radius)
				return false;

			proximity = distance / radius;
			if (pathPosition > 1)
				proximity = proximity.Map (0f, 1f, 0.5f, 1f);

			nearest = originRot;

			return true;
		}
	}

	Rotator rotator = new Rotator ();
	RotationPair currRot;
	RotationPair prevRot;

	public Vector3 ptA;
	public Vector3 ptB;
	public Vector3 ptC;

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
	Quaternion moverRotation;

	Vector3 fromPosition = Vector3.zero;
	Vector3 toPosition = Vector3.zero;

	List<Vector3> Points {
		get { return new List<Vector3> () { ptA, ptB, ptC }; }
	}

	void Awake () {
		ptA = Vector3.zero;
		UpdateBPosition ();
		UpdateCPosition ();
		mover.z = ghost.z - radius;
		moverRotation = Quaternion.LookRotation (mover - ghost);
		fromPosition = GetPointAroundAxis (ptA, moverRotation.eulerAngles.y);
		rotator.Init (fromPosition);

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
				SetMultiplePath (4);
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
		GizmosDrawer.Instance.Add (new GizmoSphere (ghost, 0.2f, Color.white));
		GizmosDrawer.Instance.Add (new GizmoSphere (mover, 0.25f));
		GizmosDrawer.Instance.Add (new GizmoSphere (fromPosition, 0.2f, Color.red));
		GizmosDrawer.Instance.Add (new GizmoSphere (toPosition, 0.2f, Color.green));

		/*float a = ghostRotation.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 v = new Vector3 (
			ghost.x + radius * Mathf.Sin (a),
			ghost.y,
			ghost.z + radius * Mathf.Cos (a)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (ghost, v));*/

		/*float a = moverRotation.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 v = new Vector3 (
			mover.x + radius * Mathf.Sin (a),
			mover.y,
			mover.z + radius * Mathf.Cos (a)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (mover, v));*/

		DrawClock ();
	}

	void StartMover () {
		
		if (moving) return;
		moving = true;

		Vector3 f = GetPointAtPosition (path[pathPosition]);
		Vector3 t = GetPointAtPosition (path[pathPosition+1]);
		pointPosition = path[pathPosition+1];
		Vector3 next = pathPosition+2 < path.Count-1 ? GetPointAtPosition (path[pathPosition+2]) : new Vector3 (-1, -1, -1);

		rotator.InitMovement (f, t, next);

		pathPosition ++;

		float speed = 2f;
		float distance = Vector3.Distance (f, t);
		float time = distance / speed;

		Coroutine.Start (
			time, 
			(float p) => { Move (p, f, t); }, 
			OnArriveAtPoint
		);
	}

	void Move (float t, Vector3 from, Vector3 to) {
		ghost = Vector3.Lerp (from, to, t);
		mover = rotator.GetPosition (ghost, pathPosition);
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

	Vector3 GetPointAroundAxis (Vector3 pivot, float angle) {
		
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
		rotA = rotB;
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