using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

public class Cow : Unit, IInventoryHolder {

	public Inventory Inventory { get; set; }

	void Awake () {
		Inventory = new Inventory ();
		Inventory.Add (new IceCreamHolder ());
		List<Item> items = new List<Item> ();
		items.Add (new IceCreamItem (Flavor.Chocolate));
		items.Add (new IceCreamItem (Flavor.Vanilla));
		items.Add (new IceCreamItem (Flavor.Chocolate));
		items.Add (new IceCreamItem (Flavor.Chocolate));
		Inventory.AddItems<IceCreamHolder> (items);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.X)) {
			Inventory.Print ();
		}
	}
}
