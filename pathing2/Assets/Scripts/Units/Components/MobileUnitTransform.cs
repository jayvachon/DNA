using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		List<PathPoint> previousPath; // TODO: instead of doing this, picking up a unit should automatically remove one of the points from the path

		protected override void Awake () {
			base.Awake ();
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

		public bool StartMovingOnPath (bool checkIfPreviousPath=false) {
			if (checkIfPreviousPath && PathPoints.PathsEqual (previousPath, Path.Points.Points)) {
				StopMovingOnPath ();
				return false;
			}
			Path.StartMoving ();
			previousPath = Path.Points.Points;
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
	}
}