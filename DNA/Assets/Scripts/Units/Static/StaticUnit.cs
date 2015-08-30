using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;
using GameActions;

namespace Units {

	public class StaticUnit : Unit, IActionAcceptor {

		StaticUnitTransform staticTransform;
		public StaticUnitTransform StaticTransform {
			get {
				if (staticTransform == null) {
					staticTransform = unitTransform as StaticUnitTransform;
				}
				return staticTransform;
			}
		}
		
		public virtual bool PathPointEnabled {
			get { return true; }
		}

		public PathPoint PathPoint { get; set; }
		public AcceptableActions AcceptableActions { get; protected set; }

		protected void Destroy<T> (bool enablePathPoint=true) where T : StaticUnit {
			if (enablePathPoint) {
				StaticUnit plot = ObjectCreator.Instance.Create<Plot> (Vector3.zero).GetScript<Plot> () as StaticUnit;
				plot.Position = Position;
				plot.PathPoint = PathPoint;
				PathPoint.StaticUnit = plot;
				if (Selected) SelectionManager.Select (plot.UnitClickable);
			} else {
				ObjectCreator.Instance.Destroy<PathPoint> (PathPoint.MyTransform);
			}
			ObjectCreator.Instance.Destroy<T> (transform);
		}
	}
}