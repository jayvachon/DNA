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
		IActionAcceptor BoundAcceptor { get { return mobile.BoundAcceptor; } }

		public UnitClickable ClickedUnit { get; set; }

		MobileUnit mobile;

		public MoveOnPath (MobileUnit mobile) : base (0, false, false) {
			this.mobile = mobile;
		}

		public override void Start () {
			if (ClickedUnit == null) return;
			
			Path.Points.Clear ();
			Path.Points.Add (((StaticUnit)BoundAcceptor).PathPoint);
			Path.Points.Add (ClickedUnit.StaticUnit.PathPoint);

			Performer.PerformableActions.PairActionsBetweenAcceptors (
				Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));

			Path.StartMoving ();
			mobile.MobileTransform.PathRotator.StartMoving (false);

			performing = true;
		}

		public override void Stop () {
			Path.StopMoving ();
			performing = false;
		}
	}
}