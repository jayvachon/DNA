using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Tasks;

public class MockTaskAcceptor : MonoBehaviour, IInventoryHolder, ITaskAcceptor {

	AcceptableTasks acceptableTasks = null;
	public AcceptableTasks AcceptableTasks { 
		get {
			if (acceptableTasks == null) {
				acceptableTasks = new AcceptableTasks (this);
				AcceptableTasks.Add (new AcceptCollectItemTest<YearHolder> ());
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
				inventory.Add (new YearHolder (5, 5));
			}
			return inventory;
		}
	}

	public void ClearHolder<T> () where T : ItemHolder {
		T holder = Inventory.Get<T> ();
		holder.Clear ();
	}

	public void FillHolder<T> () where T : ItemHolder {
		T holder = Inventory.Get<T> ();
		holder.Initialize (5);
	}

	public void HalfFillHolder<T> () where T : ItemHolder {
		T holder = Inventory.Get<T> ();
		holder.Clear ();
		holder.Add (3);
	}
}
