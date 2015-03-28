using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path path;
		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		MobileUnit mobileUnit = null;
		MobileUnit MobileUnit {
			get {
				if (mobileUnit == null) {
					mobileUnit = Unit as MobileUnit;
				}
				return mobileUnit;
			}
		}

		protected override void Awake () {
			base.Awake ();
			Path = path;
			path.Init (this);
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public void StartMovingOnPath () {
			Path.StartMoving ();
		}

		public void StopMovingOnPath () {
			Path.StopMoving ();
		}

		public void ArriveAtPoint (PathPoint point) {
			StaticUnitTransform unitTransform = point.unit;
			if (unitTransform == null) {
				StartMovingOnPath ();
			} else {
				MobileUnit.OnBindActionable (unitTransform.Unit as IActionAcceptor);
			}
		}
	}
}