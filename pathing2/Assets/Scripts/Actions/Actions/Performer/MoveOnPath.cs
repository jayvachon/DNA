using UnityEngine;
using System.Collections;
using Units;
using Pathing;

namespace GameActions {

	public class MoveOnPath : PerformerAction {

		public override string Name {
			get { return "MoveOnPath"; }
		}

		Path Path { get { return mobile.Path; } }
		MobileUnit mobile;

		public MoveOnPath (MobileUnit mobile) : base (0, false, false) {
			this.mobile = mobile;
		}

		public override void Start () {
			
			//Performer.PerformableActions.PairActionsBetweenAcceptors (
			//	Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));

			Path.StartMoving ();

			performing = true;
		}

		public override void Stop () {
			Path.StopMoving ();
			performing = false;
		}
	}
}