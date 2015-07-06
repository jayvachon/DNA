using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path Path { get; set; }

		float progress;
		public float Progress {
			get { return progress; }
			set {
				progress = value + 0.25f;
				if (progress > 1f) progress -= 1f;
				LocalPosition = new Vector3 (
					GetX (progress), 
					GetY (Mathf.Abs (1f-progress)),
					GetZ (progress));
			}
		}

		PathRotator pathRotator;
		public PathRotator PathRotator {
			get {
				if (pathRotator == null) {
					pathRotator = MyTransform.GetParentOfType<PathRotator> ();
				}
				return pathRotator;
			}
		}

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

		protected override void Awake () {
			base.Awake ();
			TWO_PI = Mathf.PI * 2f;
			yPos = Position.y;
			Path = ObjectCreator.Instance.Create<Path> ().GetScript<Path> ();
			Path.MyTransform.SetParent (MobileUnit.transform);
			Path.Init (this, new PathSettings (2, false), PathRotator);
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public void StopMovingOnPath () {
			movementState = MovementState.Idling;
			//Path.StopMoving ();
			MobileUnit.PerformableActions.Stop ("MoveOnPath");
		}

		// On arrive at point:
		// A. point has action
		//		a. action requires pair
		//			i. pair exists on path
		//				- perform action & move to pair
		//			ii. pair does not exist on path
		//				a. pair exists in world
		//					- make new path w/ pair, perform action, & move to pair
		//				b. pair does not exist in world
		//					- do not perform action
		//		b. action does not require pair
		//			- perform action
		// √ B. point does not have action
		//		√ - do not perform action

		public void ArriveAtPoint (PathPoint point) {
			//PerformableActions.PairActionsOnPath (Path);
			
			/*List<string> paired = PerformableActions.GetPairedActionsBetweenAcceptors (
				point.StaticUnit as IActionAcceptor,
				Path.Points.Points.Find (x => x != point).StaticUnit as IActionAcceptor);

			List<string> bound = PerformableActions.GetBoundActions (paired);
			if (bound.Count > 0) {
				PerformableActions.Start (bound[0]);
			}*/

			if (MobileUnit.OnBindActionable (point))
				EncircleBoundUnit ();

			/*AcceptorAction a = point.StaticUnit.AcceptableActions.GetActiveAction ();
			StaticUnitTransform unitTransform = point.StaticUnitTransform;
			if (a == null || !a.Enabled) {
				ResetPath ();
			} else {
				if (a.EnabledState.RequiresPair) {
					if (!point.PointsHavePairs (Path.Points.Points)) {
						ResetPath ();
						PathPoint nearest = Pathfinder.Instance.FindNearestWithAction (point.Position, a.EnabledState.RequiredPair);
						Debug.Log (nearest);
						if (nearest != null) {
							Path.Points.Add (point);
							Path.Points.Add (nearest);
							MobileUnit.PerformableActions.PairActionsBetweenAcceptors (
								Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
						} 
					}
				} else {
					ResetPath ();
				}
			}
			MobileUnit.OnBindActionable ((IActionAcceptor)unitTransform.Unit);
			if (MobileUnit.PerformableActions.Performing) {
				EncircleBoundUnit ();
			}*/
		}

		void ResetPath () {
			Path.Points.Clear ();
			StopMovingOnPath ();
		}

		public void EncircleBoundUnit () {
			if (movementState == MovementState.Working) return;
			movementState = MovementState.Working;
			StaticUnit su = (StaticUnit)BoundAcceptor;
			StartCoroutine (CoEncircleBoundUnit (su.Position));
		}

		// TODO: clean up
		IEnumerator CoEncircleBoundUnit (Vector3 center) {
			
			float p = 0f;
			float sign = Mathf.Sign (LocalPosition.x);
			float offset = Parent.localEulerAngles.y + ((sign > 0) ? 90f : 270f);
			float speed = Path.Speed * xMax * 2f; // Path speed * diameter

			while (p < 1f || MobileUnit.PerformableActions.Performing && BoundAcceptor != null) {
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
			movementState = (Path.Points.Count > 1)
				? MovementState.Moving
				: MovementState.Idling;
		}

		// TODO: Move this to MoveOnPath action
		float GetZ (float p) {
			return xMax * Mathf.Sin (TWO_PI * p * 2f);
		}

		// TODO: Move this to MoveOnPath action
		float GetX (float p) {
			return (PathRotator.Distance + xMax * 2) * Mathf.Sin (TWO_PI * p) / 2f;
		}

		float GetY (float p) {
			float y = PathRotator.GetYPosition (p);
			if (y == -1) return yPos;
			return y;
		}
	}
}