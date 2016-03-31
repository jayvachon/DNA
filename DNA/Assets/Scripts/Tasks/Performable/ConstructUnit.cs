using UnityEngine;
using System.Linq;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {
	
	public class ConstructUnit : CostTask, IConstructable {

		public PathElementContainer ElementContainer { get; set; }

		public ConstructUnit (Inventory inventory=null) : base (inventory) {}

		public virtual bool CanConstruct (PathElement element) {
			if (element is Connection)
				return false;
			return CanAfford && Settings.ConstructionTargets.Any (((GridPoint)element).Unit.Settings.Symbol.Contains) && element.State == DevelopmentState.Undeveloped;
		}
	}

	public class ConstructUnit<T> : ConstructUnit where T : StaticUnit {

		public override bool Enabled {
			get { return CanAfford && DataManager.GetUnitSettings (typeof (T)).Unlocked; }
		}

		protected override void OnEnd () {
			Purchase ();
			try {
				ElementContainer.BeginConstruction<T> (TotalCost);
			} catch {
				throw new System.Exception ("The path element container has not been set for the task '" + this + ".' Be sure to set the property ElementContainer");
			}
			base.OnEnd ();
		}
	}
}