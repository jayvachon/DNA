using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable, IBinder {

		public Path path;
		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

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

		public void StartMoveOnPath () {
			Path.Move ();
		}

		public void ArriveAtPoint (IPathPoint point) {
			if (point is IActionAcceptor) {
				OnBindActionable (point as IActionAcceptor);
			} else {
				StartMoveOnPath ();
			}
		}

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			StartMoveOnPath ();
		}
	}
}