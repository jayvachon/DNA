using UnityEngine;
using System.Collections;
using GameInventory;

// This class might not be necessary

public static class Player {

	static Inventory inventory;
	static Inventory Inventory {
		get {
			if (inventory == null) {
				inventory.Add (new MilkshakeHolder (100000, 30));
			}
			return inventory;
		}
	}

	public static MilkshakeHolder Milkshakes {
		get { return (MilkshakeHolder)Inventory["Milkshakes"]; }
	}
}
