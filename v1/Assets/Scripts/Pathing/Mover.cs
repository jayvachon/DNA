using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {

	public class Mover : MBRefs {

		IPathable pathable;
		PathPoints pathPoints;
		int position = 0;
		float speed = 10f;
		bool moving = false;
		bool forward = true;

		List<Vector3> Positions {
			get { return pathPoints.Positions; }
		}

		int PositionsCount {
			get { return Positions.Count; }
		}

		IPathPoint CurrentPoint {
			get { return pathPoints.Points[forward ? position : position-1]; }
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
				if (pathPoints.Count > 1) {
					IteratePosition ();
					Vector3[] line = new Vector3[] { PrevPosition, NextPosition };
					return line;
				}
				return null;
			}
		}

		public static Mover Create (IPathable pathable, PathPoints pathPoints) {
			
			GameObject go = new GameObject ("Mover", typeof (Mover));
			
			Transform t = go.transform;
			MonoBehaviour p = pathable as MonoBehaviour;
			t.position = p.transform.position;
			p.transform.SetParent (t);
			
			Mover mover = go.GetScript<Mover> ();
			mover.Init (pathable, pathPoints);
			return mover;
		}

		/**
		 *	Public functions
		 */

		public void Init (IPathable pathable, PathPoints pathPoints) {
			this.pathable = pathable;
			this.pathPoints = pathPoints;
		}

		public void Move () {
			if (moving) return;
			Vector3[] line = Line;
			if (line != null) {
				moving = true;
				StartCoroutine (CoMove (line));
			}
		}

		public bool CanRemovePoint (IPathPoint pathPoint) {
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

			while (eTime < time) {
				eTime += Time.deltaTime;
				MyTransform.position = Vector3.Lerp (start, end, eTime / time);
				yield return null;
			}

			moving = false;
			pathable.ArriveAtPoint (CurrentPoint);
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
				if (pathPoints.Loop) {
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
				if (pathPoints.Loop) {
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