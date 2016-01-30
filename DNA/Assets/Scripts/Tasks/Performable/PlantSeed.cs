using UnityEngine;
using System.Linq;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {
	
	public class PlantSeed : PerformerTask, IConstructable {

		public bool CanConstruct (PathElement element) {
			return true;
		}

		protected override void OnEnd () {
			try {
				ElementContainer.BeginConstruction<Flower> ().LaborCost = 10;
			} catch {
				throw new System.Exception ("The path element container has not been set for the task '" + this + ".' Be sure to call SetConstructionPoint (container)");
			}
			base.OnEnd ();
		}
	}
}