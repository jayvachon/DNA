using UnityEngine;
using System.Linq;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {
	
	public class PlantSeed : PerformerTask, IConstructable {

		public PathElementContainer ElementContainer { get; set; }

		public bool CanConstruct (PathElement element) {
			return true;
		}

		protected override void OnEnd () {
			try {
				// TODO: don't hardcode this value (time to grow flower)				
				ElementContainer.BeginConstruction<Flower> (60, true);
			} catch {
				throw new System.Exception ("The path element container has not been set for the task '" + this + ".' Be sure to set the property ElementContainer");
			}
			base.OnEnd ();
		}
	}
}