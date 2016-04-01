using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.InputSystem;
using InventorySystem;

namespace DNA.Tasks {

	public class CancelConstruction : InventoryTask {

		PathElementContainer container;
		string buildingSymbol;

		public CancelConstruction (Inventory inventory) : base (inventory) {}

		public void Init (PathElementContainer container, string buildingSymbol) {
			this.container = container;
			this.buildingSymbol = buildingSymbol;
		}

		public override bool Enabled {
			get { return true; }
		}

		protected override void OnEnd () {

			container.CancelConstruction ();
			SelectionHandler.Clear ();

			foreach (var cost in DataManager.GetConstructionCosts (buildingSymbol))
				Inventory[cost.Key].Add (cost.Value);

			base.OnEnd ();
		}
	}
}