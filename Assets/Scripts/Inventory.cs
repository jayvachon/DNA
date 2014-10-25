using UnityEngine;
using System.Collections;

public class Inventory : System.Object {

//	Dictionary<string> items = new Dictionary<string>(); TODO make this a dictionary
	InventoryItem[] items;

	public Inventory (InventoryItem[] items) {
		this.items = items;
	}

	public bool Add (string name, int amount) {
		return GetItem (name).Add (amount);
	}

	public int GetAmount (string name) {
		return GetItem (name).Amount;
	}

	InventoryItem GetItem (string name) {
		for (int i = 0; i < items.Length; i ++) {
			InventoryItem ii = items[i];
			if (ii.Name == name) {
				return ii;
			}
		}
		return null;
	}
}
