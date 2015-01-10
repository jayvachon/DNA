using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory : System.Object {

	List<InventoryItem> items = new List<InventoryItem>();
	int maxCapacity;

	public Inventory (int maxCapacity=1) {
		this.maxCapacity = maxCapacity;
	}

	public bool Add (InventoryItem item) {
		if (items.Count < maxCapacity && !Has (item)) {
			items.Add (item);
			return true;
		}
		return false;
	}

	public void Remove<T> () where T : InventoryItem {
		items.Remove (Get<T> ());
	}

	public bool Has (InventoryItem item) {
		return items.Contains (item);
	}

	public InventoryItem Get<T> () where T : InventoryItem {
		foreach (InventoryItem item in items) {
			if (item is T)
				return item;
		}
		return null;
	}

	// Trade always gets called by the receiving IInventoryHolder
	public bool Trade<T> (IInventoryHolder sender, int amount) where T : InventoryItem {
		T senderItem = sender.MyInventory.Get<T> () as T;
		T myItem = Get<T> () as T;
		if (senderItem == null || myItem == null) {
			return false;
		} else {
			int sub = senderItem.GetSubtract (amount);
			int add = myItem.GetAdd (sub);
			senderItem.Add (add);
			return true;
		}
	}
}
