using UnityEngine;
using System.Collections;

public class Person : Unit {

	Inventory inventory;
	IceCream icDestination = null;
	int count = 0;

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
		/*if (inventory.Add ("ice cream", 1)) {
			ic.Collect ();
		}*/
		icDestination = ic;
		StartMove (ic.transform.position);
//		StartMove (icDestination.transform.position);
	}

	public override void OnEndMove () {
		if (icDestination == null) return;
		if (Vector3.Distance (MyTransform.position, icDestination.transform.position) < 5) {
			if (inventory.Add ("ice cream", 1)) {
				icDestination.Collect ();
			}
		}
	}
}