using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Paths;
using DNA.Tasks;

namespace DNA.Units {

	public class ConstructionSite : StaticUnit, IPathElementObject {

		void Awake () {
			Inventory = new Inventory (this);
			Inventory.Add (new LaborHolder (1000, 10)).HolderEmptied += OnEndConstruction;
			AcceptableTasks.Add (new AcceptCollectItem<LaborHolder> ());
		}

		void OnEndConstruction () {
			Debug.Log (":)");
		}
	}
}