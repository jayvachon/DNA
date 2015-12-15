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

	public class Trajectory {

		public readonly float OriginArc;
		public readonly float TargetArc;
		public readonly Vector3 GhostStart;
		public readonly Vector3 GhostEnd;
		public readonly Vector3 Target;
		public readonly bool TargetIsEnd;
		public readonly Vector3 Origin;

		public Trajectory (
			float radius,
			Vector3 origin, 
			Vector3 target, 
			Quaternion originRotationFrom, 
			Quaternion originRotationTo, 
			Quaternion targetRotationFrom,
			Quaternion targetRotationTo,
			bool targetIsEnd) {

			Origin = origin;
			OriginArc = originRotationFrom.ArcLengthClockwise (originRotationTo, radius);
			TargetArc = targetRotationFrom.ArcLengthClockwise (targetRotationTo, radius);
			GhostStart = origin.GetPointAroundAxis (originRotationTo.eulerAngles.y);
			GhostEnd = target.GetPointAroundAxis (targetRotationFrom.eulerAngles.y);
			Target = target;
			TargetIsEnd = targetIsEnd;
		}
	}

	Transform mover;

	RotationPair targetRot;
	RotationPair originRot;
	public RotationPair nearest;
	Vector3 prevPosition;
	float proximity;
	float radius = 1f;

	public PathRotator (Transform mover, Vector3 initialPoint, float radius=1f) {
		this.mover = mover;
		this.radius = radius;
		Quaternion moverRotation = Quaternion.LookRotation (mover.position - initialPoint);
		prevPosition = initialPoint.GetPointAroundAxis (moverRotation.eulerAngles.y);
	}

	public Trajectory InitMovement (Vector3 origin, Vector3 target, Vector3 next, bool end) {

		Quaternion to = Quaternion.identity;
		Quaternion from = Quaternion.identity;
		
		if (end) {
			to = Quaternion.LookRotation (origin - target);
			from = to;
		} else {
			to = Quaternion.LookRotation (next - target); 		// set "to" rotation to look at the next point
			from = Quaternion.LookRotation (origin - target); 	// set "from" rotation to look at the previous point
		}

		// Set target rotation pair
		targetRot = new RotationPair (target, from, to, end);

		// Set origin rotation pair
		to = Quaternion.LookRotation (target - origin);
		from = to;
		originRot = new RotationPair (origin, from, to, false);

		return new Trajectory (radius, origin, target,
			Quaternion.LookRotation (prevPosition - originRot.Pivot),
			to,
			targetRot.From,
			targetRot.To,
			targetRot.EndPoint
		);
	}

	public void ApplyPosition (Vector3 ghostPosition, int pathPosition) {
		
		bool approachingTarget = OnApproachTarget (ghostPosition);
		bool departingOrigin = OnDepartOrigin (ghostPosition, pathPosition);

		if (!approachingTarget && !departingOrigin) {
			mover.position = ghostPosition;
			return;
		} else {

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

	public void RotateAroundPoint (float p) {
		Quaternion q = nearest.From.SlerpClockwise (nearest.To, p);
		mover.position = nearest.Pivot.GetPointAroundAxis (q.eulerAngles.y);
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
		if (pathPosition > 0)
			proximity = proximity.Map (0f, 1f, 0.5f, 1f);

		nearest = originRot;

		return true;
	}
}