using UnityEngine;
using System.Collections;
using Pathing;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable, IPathMover {

		public Path Path { get; set; }

		protected override void Awake () {
			base.Awake ();
			Path = Path.Create (this, this);
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public void StartMoveOnPath () {}
		public void ArriveAtPoint (IPathPoint point) {}
	}
}