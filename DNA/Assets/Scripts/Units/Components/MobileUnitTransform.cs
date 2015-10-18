using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class MobileUnitTransform : UnitTransform {

		enum MovementState {
			Idling, Moving, Working
		}

		MovementState movementState = MovementState.Idling;

		public bool Working {
			get { return movementState == MovementState.Working; }
		}

		float xMax = 1.25f;
		float TWO_PI;
		float yPos;
		bool performing = false;

		protected override void Awake () {
			base.Awake ();
			TWO_PI = Mathf.PI * 2f;
			yPos = Position.y;
		}

		/*public void StopMovingOnPath () {
			movementState = MovementState.Idling;
			//MobileUnit.PerformableActions.Stop ("MoveOnPath");
		}*/

		//public bool ArriveAtPoint (PathPoint point) {
			/*if (MobileUnit.OnArriveAtPoint (point)) {
				performing = true;
				EncircleBoundUnit (false, point.StaticUnit.Position);
				return true;
			}*/
		//	return false; // TODO: don't return anything

			/*if (MobileUnit.OnBindActionable (point)) {
				EncircleBoundUnit ();
				return true;
			}
			return false;*/
		//}

		public void OnCompleteTask () {
			performing = false;
		}

		public void EncircleBoundUnit (bool overridePosition, Vector3 position) {
			if (movementState == MovementState.Working) return;
			movementState = MovementState.Working;
			if (overridePosition) {
				//StaticUnit su = (StaticUnit)BoundAcceptor;
				//StartCoroutine (CoEncircleBoundUnit (su.Position));
			} else {
				StartCoroutine (CoEncircleBoundUnit (position));
			}
		}

		// TODO: clean up
		IEnumerator CoEncircleBoundUnit (Vector3 center) {
			
			float p = 0f;
			float sign = Mathf.Sign (LocalPosition.x);
			float offset = Parent.localEulerAngles.y + ((sign > 0) ? 90f : 270f);
			float speed = 5f;//Path.Speed * xMax * 2f; // Path speed * diameter

			//while (p < 1f || MobileUnit.PerformableActions.Performing && BoundAcceptor != null) {
			while (p < 1f || performing) {
				if (p >= 1f) p = 0f;
				p += speed * Time.deltaTime;
				if (sign > 0) {
					Position = new Vector3 (
						center.x + xMax * Mathf.Sin (TWO_PI * p + offset * Mathf.Deg2Rad),
						center.y,
						center.z + xMax * Mathf.Cos (TWO_PI * p + offset * Mathf.Deg2Rad));
				} else {
					float pInv = Mathf.Abs (p-1);
					Position = new Vector3 (
						center.x + xMax * Mathf.Sin (TWO_PI * pInv + offset * Mathf.Deg2Rad),
						center.y,
						center.z + xMax * Mathf.Cos (TWO_PI * pInv + offset * Mathf.Deg2Rad));
				}
				yield return null;
			}
			/*movementState = (Path.Points.Count > 1)
				? MovementState.Moving
				: MovementState.Idling;*/
			movementState = MovementState.Moving;
		}

		// TODO: Move this to MoveOnPath action
		float GetZ (float p) {
			return xMax * Mathf.Sin (TWO_PI * p * 2f);
		}

		// TODO: Move this to MoveOnPath action
		float GetX (float p) {
			return 1f;
			//return (PathRotator.Distance + xMax * 2) * Mathf.Sin (TWO_PI * p) / 2f;
		}

		float GetY (float p) {
			//float y = PathRotator.GetYPosition (p);
			float y= 1f;
			if (y == -1) return yPos;
			return y;
		}
	}
}