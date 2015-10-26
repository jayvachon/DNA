using UnityEngine;
using System.Collections;
using InventorySystem;
using DNA.Tasks;

public class MockTaskAcceptor : MonoBehaviour, IInventoryHolder, ITaskAcceptor {

	AcceptableTasks acceptableTasks = null;
	public AcceptableTasks AcceptableTasks { 
		get {
			if (acceptableTasks == null) {
				acceptableTasks = new AcceptableTasks (this);
				AcceptableTasks.Add (new AcceptCollectItemTest<YearGroup> ());
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
				inventory.Add (new YearGroup (5, 5));
			}
			return inventory;
		}
	}

	public void ClearGroup<T> () where T : ItemGroup {
		Inventory.Get<T> ().Clear ();
	}

	public void FillGroup<T> () where T : ItemGroup {
		Inventory.Get<T> ().Fill ();
		//T group = Inventory.Get<T> ();
		//group.Initialize (5);
	}

	public void HalfFillGroup<T> () where T : ItemGroup {
		Inventory.Get<T> ().Set (3);
		//T group = Inventory.Get<T> ();
		//group.Clear ();
		//group.Add (3);
	}
}
