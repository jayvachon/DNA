using UnityEngine;
using System.Collections;

public class Person : Unit, IInventoryHolder {

	public Unit boundUnit;
	public IInventoryHolder boundInventory;
	public Inventory MyInventory { get; set; }

	void Awake () {
		MyInventory = new Inventory ();
		MyInventory.Add (new IceCreamItem (5, 0));
		boundInventory = boundUnit as IInventoryHolder;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			MyInventory.Trade<IceCreamItem> (boundInventory, 2);
		}
	}
}
