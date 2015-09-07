using UnityEngine;
using System.Collections;
using GameInventory;
using DNA.Tasks;

public class MockTaskAcceptor2 : MonoBehaviour, IInventoryHolder, ITaskAcceptor {

	AcceptableTasks acceptableTasks = null;
	public AcceptableTasks AcceptableTasks { 
		get {
			if (acceptableTasks == null) {
				acceptableTasks = new AcceptableTasks (this);
				//AcceptableTasks.Add (new AcceptCollectItemTest<YearHolder> ());
				AcceptableTasks.Add (new AcceptDeliverItemTest<YearHolder> ());
			}
			return acceptableTasks;
		}
	}

	Inventory inventory;
	public Inventory Inventory { 
		get {
			if (inventory == null) {
				inventory = new Inventory (this);
				inventory.Add (new YearHolder (5, 0));
			}
			return inventory;
		}
	}
}
