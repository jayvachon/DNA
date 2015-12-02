using UnityEngine;
using System.Collections;

public class PathRotator {

	public class RotationPair {

		public readonly Vector3 Pivot;
		public readonly Quaternion From;
		public readonly Quaternion To;
		public readonly bool EndPoint;

		Vector3? fromPosition = null;
		public Vector3 FromPosition {
			get {
				if (fromPosition == null) {
					fromPosition = Pivot.GetPointAroundAxis (From.eulerAngles.y);
				}
				return (Vector3)fromPosition;
			}
		}

		Vector3? toPosition = null;
		public Vector3 ToPosition {
			get {
				if (toPosition == null) {
					toPosition = Pivot.GetPointAroundAxis (To.eulerAngles.y);
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
	}

	Transform mover;
	Transform ghost;

	RotationPair targetRot;
	RotationPair originRot;
	RotationPair nearest;
	Vector3 prevPosition;
	float proximity;
	float radius = 1f;
	bool insideRadius = false;

	public Vector3 Position { get; set; }

	// TODO: pass in transform instead & set the start position here
	/*public void Init (Vector3 startPosition) {
		prevPosition = startPosition;
	}*/

	public PathRotator (Transform mover, Transform ghost, Vector3 initialPoint) {
		this.mover = mover;
		this.ghost = ghost;
		Quaternion moverRotation = Quaternion.LookRotation (mover.position - ghost.position);
		prevPosition = initialPoint.GetPointAroundAxis (moverRotation.eulerAngles.y);
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

		Debug.Log (from.ArcLengthClockwise (to, radius));

		originRot = new RotationPair (origin, from, to, false);
	}

	public void ApplyPosition (int pathPosition) {
		
		bool approachingTarget = OnApproachTarget ();
		bool departingOrigin = OnDepartOrigin (pathPosition);

		if (!approachingTarget && !departingOrigin) {
			mover.position = ghost.position;
			insideRadius = false;
			return;
		} else {

			insideRadius = true;

			// don't spin if approaching the final target
			if (nearest.EndPoint) {
				mover.position = nearest.ToPosition;
				return;
			}

			Quaternion from = Quaternion.LookRotation (prevPosition - nearest.Pivot);

			// special case that prevents spinning around the point we're starting from if already pointing in the right direction
			if (departingOrigin && pathPosition == 1 && nearest.To == from) {
				mover.position = nearest.ToPosition;
				return;
			}

			Quaternion q = from.SlerpClockwise (nearest.To, proximity);

			mover.position = nearest.Pivot.GetPointAroundAxis (q.eulerAngles.y);
		}
	}

	public float GetStepSize (float speed) {
		if (!insideRadius)
			return speed;
		return speed;
	}

	bool OnApproachTarget () {

		float distance = Vector3.Distance (ghost.position, targetRot.Pivot);
		if (distance > radius)
			return false;

		proximity = (distance / radius).Map (1f, 0f, 0f, 0.5f);
		nearest = targetRot;
		prevPosition = targetRot.FromPosition;

		return true;
	}

	bool OnDepartOrigin (int pathPosition) {

		float distance = Vector3.Distance (ghost.position, originRot.Pivot);
		if (distance > radius)
			return false;

		proximity = distance / radius;
		if (pathPosition > 1)
			proximity = proximity.Map (0f, 1f, 0.5f, 1f);

		nearest = originRot;

		return true;
	}
}