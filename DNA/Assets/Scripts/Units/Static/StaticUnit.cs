using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using Pathing;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class StaticUnit : Unit, IPathElementObject, ITaskAcceptor {

		public PathElement Element { get; set; }

		public PathPoint PathPoint { get; set; }*/

		AcceptableTasks acceptableTasks;
		public AcceptableTasks AcceptableTasks {
			get {
				if (acceptableTasks == null) {
					acceptableTasks = new AcceptableTasks (this);
				}
				return acceptableTasks;
			}
		}

		// TODO: update this to work with new points
		// this happens e.g. when coffee runs out of resources
		protected void Destroy<T> (bool enablePathPoint=true) where T : StaticUnit {
			Debug.Log ("destroy");
			if (enablePathPoint) {
				StaticUnit plot = ObjectPool.Instantiate<Plot> () as StaticUnit;
				plot.Position = Position;
				if (Selected) SelectionManager.Select (plot.UnitClickable);
			} 
			DestroyThis<T> ();
		}
	}
}