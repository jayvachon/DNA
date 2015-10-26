using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

public class MockTaskAcceptor2 : MonoBehaviour, IInventoryHolder, ITaskAcceptor {

	AcceptableTasks acceptableTasks = null;
	public AcceptableTasks AcceptableTasks { 
		get {
			if (acceptableTasks == null) {
				acceptableTasks = new AcceptableTasks (this);
				//AcceptableTasks.Add (new AcceptCollectItemTest<YearHolder> ());
				AcceptableTasks.Add (new AcceptDeliverItemTest<YearGroup> ());
			}
			return acceptableTasks;
		}
	}

	Inventory inventory;
	public Inventory Inventory { 
		get {
			if (inventory == null) {
				inventory = new Inventory (this);
				inventory.Add (new YearGroup (0, 5));
			}
			return inventory;
		}
	}
}
