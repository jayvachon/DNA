using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	public class PathPositioner : PathComponent {

		int position = 0;
		bool moving = false;
		bool forward = true;
		
		float speed = 5;
		public float Speed {
			get { return speed; }
			set { speed = value; }
		}

		public float Progress { get; private set; }
		
		List<Vector3> Positions {
			get { return Points.Positions; }
		}

		int PositionsCount {
			get { return Positions.Count; }
		}

		PathPoint CurrentPoint {
			get { return Points.Points[forward ? position : position-1]; }
		}

		Vector3 PrevPosition {
			get { 
				if (forward) {
					return Positions[Mathf.Max (0, position-1)];
				} else {
					return Positions[position];
				}
			}
		}

		Vector3 NextPosition {
			get { 
				if (forward) {
					return Positions[position]; 
				} else {
					return Positions[Mathf.Max (0, position-1)];
				}
			}
		}

		Vector3[] Line {
			get {
				if (Points.Count > 1) {
					IteratePosition ();
					Vector3[] line = new Vector3[] { PrevPosition, NextPosition };
					return line;
				}
				return null;
			}
		}

		public void StartMoving () {
			if (moving) return;
			Vector3[] line = Line;
			if (line != null) {
				moving = true;
				StartCoroutine (CoMove (line));
			}
		}

		public void StopMoving () {
			moving = false;
		}

		public bool CanRemovePoint (PathPoint pathPoint) {
			Vector3 pointPosition = pathPoint.Position;
			if (moving) {
				return !pointPosition.Equals (PrevPosition) && !pointPosition.Equals (NextPosition);
			}
			if (forward) {
				return !pointPosition.Equals (PrevPosition);
			} else {
				return !pointPosition.Equals (NextPosition);
			}
		}

		/**
		 *	Private functions
		 */

		IEnumerator CoMove (Vector3[] line) {

			Vector3 start = line[0];
			Vector3 end = line[1];

			float distance = Vector3.Distance (start, end);
			float time = distance / speed;
			float eTime = 0f;

			while (eTime < time && moving) {
				eTime += Time.deltaTime;
				float t = eTime / time;
				Progress = (Mathf.Cos (t * 180f * Mathf.Deg2Rad) - 1) / -2;
				Pathable.PathPosition = Vector3.Lerp (start, end, Progress);
				yield return null;
			}

			if (moving) {
				moving = false;
				Pathable.ArriveAtPoint (CurrentPoint);
			}
		}

		void IteratePosition () {
			if (forward) {
				IterateForward ();
			} else {
				IterateBackward ();
			}
		}

		void IterateForward () {
			if (position+1 > PositionsCount-1) {
				if (Points.Loop) {
					position = 1;
				} else {
					forward = false;
				}
			} else {
				position ++;
			}
		}

		void IterateBackward () {
			if (position-1 < 1) {
				if (Points.Loop) {
					position = PositionsCount-1;
				} else {
					forward = true;
				}
			} else {
				position --;
			}
		}
	}
}