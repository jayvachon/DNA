using UnityEngine;
using System.Collections;

public class Person : Unit, IInventoryHolder {

	public Unit boundUnit;
	public IInventoryHolder boundInventory;
	public Inventory MyInventory { get; set; }

	public Inventory2 MyInventory2 { get; set; }

	void Awake () {
		MyInventory = new Inventory ();
		MyInventory.Add (new IceCreamItem (5, 0));
		boundInventory = boundUnit as IInventoryHolder;

		MyInventory2 = new Inventory2 ();
		MyInventory2.Add (new InventoryItemHolder<ElderItem> ());
		/*InventoryItemHolder<ElderItem> elderHolder = MyInventory2.Get<InventoryItemHolder<ElderItem>> ();
		List<ElderItem> elders = new List<ElderItem> ();
		elders.Add (new ElderItem ());
		elders.Add (new ElderItem ());
		elders.Add (new ElderItem ());
		elders.Add (new ElderItem ());
		elderHolder.Add (elders);*/
	}

	// Debugging
	/*void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			MyInventory.Trade<IceCreamItem> (boundInventory, 2);
		}
	}*/
}
