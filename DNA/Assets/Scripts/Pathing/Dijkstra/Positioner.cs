using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public delegate void OnArriveAtDestination (GridPoint point);

	public class Positioner {

		GridPoint destination;
		public GridPoint Destination {
			get { return destination; }
			set {
				destination = value;
				if (!moving) {
					endPoint = destination;
					StartMoving ();
				}
			}
		}

		public Vector3 PreviousPosition {
			get {
				if (Points == null)
					return Vector3.zero;
				return Points[Mathf.Max (0, pathPosition-1)];
			}
		}

		bool HasNextPoint {
			get { return path != null && path.Count > 1 && pathPosition+1 <= path.Count-1; }
		}

		List<Vector3> Points {
			get { return path.ConvertAll (x => x.Position); }
		}

		public OnArriveAtDestination OnArriveAtDestination { get; set; }

		Vector3 ghostPosition;
		Vector3 position;
		float pathAngle;
		int pathPosition = 0;
		float speed = 1.5f;
		bool moving = false;
		float radius = 1.5f;

		Transform mover;
		GridPoint startPoint;
		GridPoint endPoint;
		List<GridPoint> path;

		public Positioner (Transform mover, GridPoint startPoint, float speed=1.5f) {
			this.mover = mover;
			this.startPoint = startPoint;
			endPoint = startPoint;
			this.speed = speed;
		}

		public void StartMoving () {
			if (moving) return;
			CalculatePath ();
			if (HasNextPoint) {
				MoveToNextPoint ();
			}
		}

		void MoveToNextPoint () {
			
			moving = true;

			Vector3 from = Points[pathPosition];
			Vector3 to = Points[pathPosition+1];
			float distance = Vector3.Distance (from, to);
			float time = distance / speed;
			pathAngle = Vector3.Angle (from - to, Vector3.forward);

			Coroutine.Start (
				time, 
				//(float t) => { mover.position = Vector3.Lerp (from, to, t); }, 
				(float t) => { Move (t, from, to); }, 
				OnArriveAtPoint
			);
		}

		void Move (float t, Vector3 from, Vector3 to) {

			GizmosDrawer.Instance.Clear ();
			//GizmosDrawer.Instance.Add (new GizmoLine (from, to));
			//GizmosDrawer.Instance.Add (new GizmoRay (from, to.normalized));
			DrawPointInLineDirection (from, to);

			ghostPosition = Vector3.Lerp (from, to, t);
			GizmosDrawer.Instance.Add (new GizmoSphere (ghostPosition, 0.5f));

			float distanceToTarget = Mathf.Infinity;
			float distanceToOrigin = Mathf.Infinity;

			if (t > 0.5f) {
				distanceToTarget = Vector3.Distance (ghostPosition, to);
				//DrawCircleAroundPivot (to);
			} else {
				distanceToOrigin = Vector3.Distance (ghostPosition, from);
				//DrawCircleAroundPivot (from);
			}
			
			if (distanceToTarget <= radius) {
				mover.position = GetPointAroundAxis (to, 90f);
				//mover.RotateAround (to, Vector3.up, Time.deltaTime * speed*460 * Mathf.Abs (distanceToTarget / radius-1));
			} else if (distanceToOrigin <= radius) {
				mover.position = GetPointAroundAxis (from, 90f);
				//mover.RotateAround (from, Vector3.up, Time.deltaTime * speed*460 * Mathf.Abs (distanceToOrigin / radius-1));
			} else {
				position = ghostPosition;
				mover.position = position;
			}
		}

		void DrawPointInLineDirection (Vector3 pivot, Vector3 target) {

			Quaternion q = new Quaternion ();
			q = Quaternion.LookRotation (target - pivot);

			//Vector3 e = q.eulerAngles.normalized;

			float a = Vector3.Angle (target - pivot, Vector3.forward);
			float r = a * Mathf.Deg2Rad;
			Debug.Log (a);
			Vector3 position = new Vector3 (
				pivot.x + radius * Mathf.Sin (r),
				pivot.y,
				pivot.z + radius * Mathf.Cos (r)
			);

			GizmosDrawer.Instance.Add (new GizmoSphere (position, 0.25f));
		}

		void DrawCircleAroundPivot (Vector3 pivot) {
			
			int count = 4;
			float deg = 360f / (float)count;

			for (int i = 0; i < count; i ++) {
				float r = (float)i * deg * Mathf.Deg2Rad;
				Vector3 position = new Vector3 (
					pivot.x + radius * Mathf.Sin (r),
					pivot.y,
					pivot.z + radius * Mathf.Cos (r)
				);
				if (Mathf.Approximately (0f, deg * i)) {
					GizmosDrawer.Instance.Add (new GizmoLine (pivot, position));
				}
				GizmosDrawer.Instance.Add (new GizmoSphere (position, 0.2f));
			}
		}

		Vector3 GetPointAroundAxis (Vector3 pivot, float angle) {

			float sign = Mathf.Sign (pivot.x);
			//float offset = pathAngle + ((sign > 0) ? 90f : 270f);



			//float r = (pathAngle + angle) * Mathf.Deg2Rad;
			return new Vector3 (
				pivot.x + radius * Mathf.Sin (Mathf.PI * 2 * Mathf.Deg2Rad),
				pivot.y,
				pivot.z + radius * Mathf.Cos (Mathf.PI * 2 * Mathf.Deg2Rad)
			);
		}

		void CalculatePath () {

			if (path != null && path.Count > 0)
				startPoint = path[pathPosition];

			if (startPoint == endPoint)
				return;
				
			List<GridPoint> newPath = Pathfinder.GetFreePath (startPoint, endPoint);
			if (newPath.Count > 0) {
				path = newPath;
				pathPosition = 0;
			}
		}

		void OnArriveAtPoint () {
			moving = false;
			pathPosition ++;

			// Check if the destination has been updated
			if (endPoint != Destination) {
				endPoint = Destination;
				CalculatePath ();
			}

			if (HasNextPoint) {
				MoveToNextPoint ();
			} else {
				SendArriveAtDestinationMessage ();
			}
		}

		void SendArriveAtDestinationMessage () {
			if (OnArriveAtDestination != null) 
				OnArriveAtDestination (path[path.Count-1]);
		}
	}
}