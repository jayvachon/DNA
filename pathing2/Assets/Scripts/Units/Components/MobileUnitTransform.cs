using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		Path currentPath;
		Path targetPath;

		protected override void Awake () {
			
			base.Awake ();

			currentPath = ObjectCreator.Instance.Create<Path> ().GetScript<Path> ();
			currentPath.transform.SetParent (MobileUnit.transform); 
			Path = currentPath;
			currentPath.Init (this);

			targetPath = ObjectCreator.Instance.Create<Path> ().GetScript<Path> ();
			targetPath.transform.SetParent (MobileUnit.transform);
			targetPath.Init (this);
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