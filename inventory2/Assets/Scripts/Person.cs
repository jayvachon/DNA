using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

public class Person : Unit, IInventoryHolder {

	public Inventory Inventory { get; set; }
	public Cow boundCow;
	Inventory boundInventory;

	void Awake () {
		boundInventory = boundCow.Inventory;
		Inventory = new Inventory ();
		Inventory.Add (new IceCreamHolder ());
	}

	/**
	 *	Debugging
	 */

	void Update () {

		// Numbers
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			IceCreamHolder holder = Inventory.Get<IceCreamHolder> () as IceCreamHolder;
			holder.TransferFlavor (boundInventory, Flavor.Chocolate);
		}
		
		// Top row: Add
		if (Input.GetKeyDown (KeyCode.Q)) {
			Inventory.AddItem<IceCreamHolder> (new IceCreamItem (Flavor.Vanilla));
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			Inventory.AddItem<IceCreamHolder> (new IceCreamItem (Flavor.Chocolate));
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Inventory.AddItem<IceCreamHolder> (new IceCreamItem (Flavor.Pistachio));
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			Inventory.AddItem<IceCreamHolder> (new IceCreamItem (Flavor.Coffee));
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			List<Item> items = new List<Item> ();
			items.Add (new IceCreamItem (Flavor.Vanilla));
			items.Add (new IceCreamItem (Flavor.Vanilla));
			items.Add (new IceCreamItem (Flavor.Coffee));
			items.Add (new IceCreamItem (Flavor.Coffee));
			items.Add (new IceCreamItem (Flavor.Chocolate));
			items.Add (new IceCreamItem (Flavor.Pistachio));
			items.Add (new IceCreamItem (Flavor.Pistachio));
			Inventory.AddItems<IceCreamHolder> (items);
		}

		// 2nd row: Remove
		if (Input.GetKeyDown (KeyCode.A)) {
			Inventory.RemoveItem<IceCreamHolder> ();
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			IceCreamHolder holder = Inventory.Get<IceCreamHolder> () as IceCreamHolder;
			List<IceCreamItem> items = holder.RemoveFlavor (Flavor.Chocolate);
			Debug.Log (items.Count);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			Inventory.RemoveItems<IceCreamHolder> (3);
		}

		// Bottom row: Debug
		if (Input.GetKeyDown (KeyCode.Z)) {
			Inventory.Print ();
		}
	}
}
