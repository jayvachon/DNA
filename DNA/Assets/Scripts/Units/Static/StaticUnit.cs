using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using Pathing;
using DNA.Tasks;

namespace DNA.Units {

	public class StaticUnit : Unit, ITaskAcceptor {

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

		AcceptableTasks acceptableTasks;
		public AcceptableTasks AcceptableTasks {
			get {
				if (acceptableTasks == null) {
					acceptableTasks = new AcceptableTasks (this);
				}
				return acceptableTasks;
			}
		}

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
			DestroyThis<T> ();
		}
	}
}