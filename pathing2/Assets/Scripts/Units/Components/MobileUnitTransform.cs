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
				LocalPosition = new Vector3 (GetX (progress), yPos, GetZ (progress));
			}
		}

		PathRotator pathRotator;
		PathRotator PathRotator {
			get {
				if (pathRotator == null) {
					pathRotator = MyTransform.GetParentOfType<PathRotator> ();
				}
				return pathRotator;
			}
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
			Path.Init (this, new PathSettings (2, false));
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public bool StartMovingOnPath (bool reset=false) {
			if (Path.Points.Count < 2)
				return false;
			Path.StartMoving ();
			PathRotator.StartMoving (reset);
			return true;
		}

		public void StopMovingOnPath () {
			Path.StopMoving ();
		}

		public void ArriveAtPoint (PathPoint point) {
			StaticUnitTransform unitTransform = point.StaticUnitTransform;
			if (unitTransform == null) {
				StartMovingOnPath ();
			} else {
				MobileUnit.OnBindActionable (unitTransform.Unit as IActionAcceptor);
			}
		}

		public void EncircleBoundUnit (PerformerAction action) {
			StaticUnit su = (StaticUnit)BoundAcceptor;
			StartCoroutine (CoEncircleBoundUnit (su.Position, action));
		}

		// TODO: clean up
		IEnumerator CoEncircleBoundUnit (Vector3 center, PerformerAction action) {
			float sign = Mathf.Sign (LocalPosition.x);
			if (sign > 0) {
				float offset = 90f + Parent.localEulerAngles.y;
				while (action.Performing && BoundAcceptor != null) {
					float p = action.Progress;
					Position = new Vector3 (
						center.x + xMax * Mathf.Sin (TWO_PI * p + offset * Mathf.Deg2Rad),
						yPos,
						center.z + xMax * Mathf.Cos (TWO_PI * p + offset * Mathf.Deg2Rad));
					yield return null;
				}
			}
			if (sign < 0) {
				float offset = Parent.localEulerAngles.y + 270f;
				while (action.Performing && BoundAcceptor != null) {
					float p = Mathf.Abs (action.Progress-1);
					Position = new Vector3 (
						center.x + xMax * Mathf.Sin (TWO_PI * p + offset * Mathf.Deg2Rad),
						yPos,
						center.z + xMax * Mathf.Cos (TWO_PI * p + offset * Mathf.Deg2Rad));
					yield return null;
				}
			}
		}

		// TODO: Move this somewhere else?
		float GetZ (float p) {
			return xMax * Mathf.Sin (TWO_PI * p * 2f);
		}

		// TODO: Move this somewhere else?
		float GetX (float p) {
			return (PathRotator.Distance + xMax * 2) * Mathf.Sin (TWO_PI * p) / 2f;
		}
	}
}