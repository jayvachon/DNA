using UnityEngine;
using System.Collections;

public class House : Unit, IInventoryHolder {

	public Inventory MyInventory { get; set; }

	void Awake () {
		MyInventory = new Inventory ();
		MyInventory.Add (new ElderItem (2, 0));
	}
}
