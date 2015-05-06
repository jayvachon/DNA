using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

namespace Units {

	public class MobileUnitTransform : UnitTransform, IPathable {

		public Path Path { get; set; }
		public IActionAcceptor BoundAcceptor { get; private set; }
		
		/*Vector3 pathPosition;
		public Vector3 PathPosition {
			get { return pathPosition; }
			set { 
				pathPosition = value;
				PathRotator.Position = pathPosition;
			}
		}*/

		float progress;
		public float Progress { 
			get { return progress; }
			set {
				progress = value;
				PathRotator.Progress = progress;
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

		protected override void Awake () {
			base.Awake ();
			Path = ObjectCreator.Instance.Create<Path> ().GetScript<Path> ();
			Path.MyTransform.SetParent (MobileUnit.transform);
			Path.Init (this, new PathSettings (2, 2, false));
		}

		public override void OnSelect () {
			Path.Enabled = true;
		}

		public override void OnUnselect () {
			Path.Enabled = false;
		}

		public void StartMovingOnPath () {
			Path.StartMoving ();
			PathRotator.StartMoving ();
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