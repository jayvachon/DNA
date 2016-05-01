using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// deprecate

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

		public bool moving = false;
		bool encircling = false;
		public bool Moving { get { return moving; } }

		Vector3 ghostPosition;
		Vector3 position;
		int pathPosition = 0;
		float speed = 1.5f;
		float radius = 1f;

		Transform mover;
		GridPoint startPoint;
		GridPoint endPoint;
		List<GridPoint> path;
		PathRotator rotator;
		PathRotator.Trajectory trajectory;

		public Positioner (Transform mover, GridPoint startPoint, float startRotation=0f, float speed=0.75f) {//float speed=1.5f) {
			this.mover = mover;
			this.startPoint = startPoint;
			endPoint = startPoint;
			this.speed = speed;

			mover.position = startPoint.Position.GetPointAroundAxis (startRotation, radius);
			ghostPosition = startPoint.Position;
			rotator = new PathRotator (mover, startPoint.Position, radius);
		}

		public void RotateAroundPoint (float time) {
			encircling = true;
			/*Coroutine.StartWithCondition (time, 
				(float p) => { rotator.RotateAroundPoint (p); }, 
				() => { return encircling; },
				() => { encircling = false; }
			);*/
		}

		public void InterruptRotateAround () {
			encircling = false;
		}

		public void StartMoving () {
			
			if (moving) return;
			CalculatePath ();
			if (!HasNextPoint) return;
			
			/*Coroutine.WaitForCondition (() => { return !encircling; }, () => {
				
				Vector3 from = path[pathPosition].Position;
	 			Vector3 to = path[pathPosition+1].Position;
				Vector3 next = pathPosition+1 < path.Count-1 ? path[pathPosition+2].Position : from;

				trajectory = rotator.InitMovement (from, to, next, pathPosition+1 == path.Count-1);
				pathPosition ++;

				BeginMove ();
				Debug.Log ("begin");
			});*/
		}

		void BeginMove () {

			moving = true;

			ghostPosition = trajectory.Origin;
			Vector3 start = ghostPosition;
			Vector3 end = trajectory.GhostStart;
			float distance = trajectory.OriginArc;
			float time = Mathf.Abs (distance / speed) / 2f;

			// if mover already pointing in the right direction, snap the ghost to the mover (don't do a rotation)		
			if (Vector3.Distance (mover.position, end) < 0.1f) {
				ghostPosition = end;
				Move ();
			} else {
				
				// otherwise, do the rotation
				/*Coroutine.Start (time, (float p) => {
					ghostPosition = Vector3.Lerp (start, end, p);
					rotator.ApplyPosition (ghostPosition, pathPosition);
				}, Move);*/
			}
		}

		void Move () {

			Vector3 start = trajectory.GhostStart;
			Vector3 end = trajectory.GhostEnd;
			float distance = Vector3.Distance (start, end);
			float time = Mathf.Abs (distance / speed);

			/*Coroutine.Start (time, (float p) => {
				ghostPosition = Vector3.Lerp (start, end, p);
				rotator.ApplyPosition (ghostPosition, pathPosition);
			}, EndMove);*/
		}

		void EndMove () {

			if (trajectory.TargetIsEnd) {
				ghostPosition = trajectory.Target;
				OnArriveAtPoint ();
				return;
			}
			
			Vector3 start = trajectory.GhostEnd;
			Vector3 end = trajectory.Target;
			float distance = trajectory.TargetArc;
			float time = Mathf.Abs (distance / speed) / 2f;

			/*Coroutine.Start (time, (float p) => {
				ghostPosition = Vector3.Lerp (start, end, p);
				rotator.ApplyPosition (ghostPosition, pathPosition);
			}, OnArriveAtPoint);*/
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

			// Check if the destination has been updated
			if (endPoint != Destination) {
				endPoint = Destination;
				CalculatePath ();
			}

			if (HasNextPoint) {
				StartMoving ();
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