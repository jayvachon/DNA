using UnityEngine;
using System.Collections;

public class Person : Unit {

	Inventory inventory;

	public override void OnStart () {
		Init (Color.red, Color.magenta);
		inventory = new Inventory (new InventoryItem[2] {
			new InventoryItem ("ice cream", 0, 3),
			new InventoryItem ("milk", 0, 3)
		});
	}

	public override void ClickOther (MouseClickEvent e) {
		IceCream c = e.transform.GetComponent<IceCream>();
		if (c) {
			CollectIceCream (c);
		} else {
			Unselect ();
		}
	}

	void CollectIceCream (IceCream ic) {
		if (inventory.Add ("ice cream", 1)) {
			ic.Collect ();
		}
	}
}