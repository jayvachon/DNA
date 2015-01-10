using UnityEngine;
using System.Collections;

public class Cow : Unit, IInventoryHolder {

	public Inventory MyInventory { get; set; }

	void Awake () {
		MyInventory = new Inventory ();
		MyInventory.Add (new IceCreamItem (10, 10));
	}
}
