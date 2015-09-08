using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	public class PathPositioner : PathComponent {

		int position = 0;
		bool moving = false;
		bool forward = true;
		int positionOnLine = 0;

		public bool Moving {
			get { return moving; }
		}

		float speed = 5;
		public float Speed {
			get { return speed; }
			set { speed = value; }
		}
		
		public List<Vector3> Positions {
			get { return Points.Positions; }
		}

		int LineLength {
			get { return Positions.Count; }
		}

		public PathPoint CurrentPoint {
			get { 
				if (Path.Points.Count == 0)
					return null;
				return Points.Points[forward ? position : position-1]; 
			}
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

		public Vector3[] Line {
			get {
				if (Points.Count > 1) {
					Vector3[] line = new Vector3[] { PrevPosition, NextPosition };
					return line;
				}
				return null;
			}
		}

		public void StartMoving () {
			if (moving) return;
			//StopMoving ();
			IteratePosition ();
			if (Line != null) {
				moving = true;
				StartCoroutine (CoMove (Line));
			}
		}

		public void StopMoving () {
			position = 0;
			positionOnLine = 0;
			forward = true;
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

			float startProgress = positionOnLine / (float)LineLength;
			float endProgress = startProgress + (1f / (float)LineLength);
			float p = startProgress;

			while (p < endProgress && moving) {
				p += speed * Time.deltaTime;
				Pathable.Progress = p;
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
			if (positionOnLine < LineLength-1) {
				positionOnLine ++;
			} else {
				positionOnLine = 0;
			}
		}

		void IterateForward () {
			if (position+1 > LineLength-1) {
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
					position = LineLength-1;
				} else {
					forward = true;
				}
			} else {
				position --;
			}
		}
	}
}