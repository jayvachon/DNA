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

		bool HasNextPoint {
			get { return path != null && path.Count > 1 && pathPosition+1 <= path.Count-1; }
		}

		List<Vector3> Points {
			get { return path.ConvertAll (x => x.Position); }
		}

		public OnArriveAtDestination OnArriveAtDestination { get; set; }

		int pathPosition = 0;
		float speed = 3f;
		bool moving = false;

		Transform mover;
		GridPoint startPoint;
		GridPoint endPoint;
		List<GridPoint> path;

		public Positioner (Transform mover, GridPoint startPoint, float speed=5f) {
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

			Coroutine.Start (
				time, 
				(float t) => { mover.position = Vector3.Lerp (from, to, t); }, 
				OnArriveAtPoint
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