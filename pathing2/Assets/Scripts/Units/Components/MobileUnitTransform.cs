using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		float progress;
		public float Progress {
			get { return progress; }
			set {
				progress = value;
				//Position = Vector3.Lerp (Path.Positioner.Line[0], Path.Positioner.Line[1], value);
				LocalPosition = new Vector3 (GetX (progress), 0, GetZ (progress));
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

		float xMax = 1.5f;
		float TWO_PI;

		protected override void Awake () {
			base.Awake ();
			TWO_PI = Mathf.PI * 2f;
			Path = ObjectCreator.Instance.Create<Path> ().GetScript<Path> ();
			Path.MyTransform.SetParent (MobileUnit.transform);
			Path.Init (this, new PathSettings (8, 2, false));
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public bool StartMovingOnPath () {
			PathRotator.StartMoving ();
			Path.StartMoving ();
			return (Path.Points.Count >= 2);
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