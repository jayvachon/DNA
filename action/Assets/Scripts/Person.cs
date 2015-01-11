using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class Person : Unit, IInventoryHolder, IActionable {

	public int iceCreamCount = 0;
	public KeyCode debugKey;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	public Cow boundCow;
	Inventory boundInventory;

	void Awake () {

		Inventory = new Inventory ();
		Inventory.Add (new IceCreamHolder (5));

		ActionsList = new ActionsList ();
		ActionsList.Add (new CollectIceCream (Inventory));
	}

	void Update () {
		if (Input.GetKeyDown (debugKey)) {
			boundInventory = boundCow.Inventory;
			ActionsList.Start<CollectIceCream> (boundInventory, 1);
		}
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}
