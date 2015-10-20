using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class StaticUnit : Unit, IPathElementObject, ITaskAcceptor {

		PathElement element;
		public PathElement Element { 
			get { return element; }
			set {
				element = value;
				element.OnSetState += OnSetState;
			}
		}

		AcceptableTasks acceptableTasks;
		public AcceptableTasks AcceptableTasks {
			get {
				if (acceptableTasks == null) {
					acceptableTasks = new AcceptableTasks (this);
				}
				return acceptableTasks;
			}
		}

		int fertilityTier;
		public int FertilityTier {
			get { return fertilityTier; }
			set { 
				fertilityTier = value;
				OnSetFertility (value);
			}
		}

		protected override void OnDisable () {
			base.OnDisable ();
			if (Element != null)
				Element.OnSetState -= OnSetState;
		}

		void OnSetState (DevelopmentState state) {
			if (state == DevelopmentState.Abandoned)
				unitRenderer.SetAbandoned ();
		}

		// TODO: update this to work with new points
		// this happens e.g. when coffee runs out of resources
		protected void Destroy<T> (bool enablePathPoint=true) where T : StaticUnit {
			if (enablePathPoint) {
				StaticUnit plot = ObjectPool.Instantiate<Plot> () as StaticUnit;
				plot.Position = Position;
				if (Selected) SelectionManager.Select (plot.UnitClickable);
			}
			DestroyThis<T> ();
		}

		protected virtual void OnSetFertility (int fertility) {} 
	}
}