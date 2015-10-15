using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using Pathing;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class StaticUnit : Unit, IPathElementObject, ITaskAcceptor {

		public PathElement Element { get; set; }

		// deprecate
		StaticUnitTransform staticTransform;
		public StaticUnitTransform StaticTransform {
			get {
				if (staticTransform == null) {
					staticTransform = unitTransform as StaticUnitTransform;
				}
				return staticTransform;
			}
		}

		// TODO: deprecate
		public virtual bool PathPointEnabled {
			get { return true; }
		}

		// TODO: deprecate
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
			Debug.Log ("destroy");
			if (enablePathPoint) {
				StaticUnit plot = ObjectPool.Instantiate<Plot> () as StaticUnit;
				plot.Position = Position;
				plot.PathPoint = PathPoint;
				PathPoint.StaticUnit = plot;
				if (Selected) SelectionManager.Select (plot.UnitClickable);
			} else {
				ObjectPool.Destroy<PathPoint> (PathPoint.MyTransform);
			}
			DestroyThis<T> ();
		}
	}
}