using UnityEngine;
using System.Collections;

public class Hospital : Unit, IInventoryHolder {

	public Inventory MyInventory { get; set; }

	void Awake () {
		MyInventory = new Inventory ();
		MyInventory.Add (new ElderItem (10, 0));
	}
}
