using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory2 : System.Object {

	List<InventoryItemHolder> holders = new List<InventoryItemHolder> ();
	public int Count {
		get { return holders.Count; }
	}

	int maxCapacity;

	public Inventory2 (int maxCapacity=1) {
		this.maxCapacity = maxCapacity;
	}

	public bool Add (InventoryItemHolder holder) {
		if (Count < maxCapacity && !Has (holder)) {
			holders.Add (holder);
			return true;
		}
		return false;
	}

	public void Remove<T> () where T : InventoryItemHolder {
		holders.Remove (Get<T> ());
	}

	public bool Has (InventoryItemHolder holder) {
		return holders.Contains (holder);
	}

	public InventoryItemHolder Get<T> () where T : InventoryItemHolder {
		foreach (InventoryItemHolder holder in holders) {
			if (holder is T) 
				return holder;
		}
		return null;
	}

	//public bool Trade<T> 
}
