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
		Inventory = new PersonInventory ();
		ActionsList = new PersonActionsList (Inventory as PersonInventory);
	}

	void Update () {
		if (Input.GetKeyDown (debugKey)) {
			boundInventory = boundCow.Inventory;
			ActionsList.Start<CollectIceCream> (boundInventory, 1);
		}
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}

public class PersonInventory : Inventory {

	public PersonInventory () {
		Add (new IceCreamHolder (5));
	}
}

public class PersonActionsList : ActionsList {

	public PersonActionsList (PersonInventory personInventory) {
		Add (new CollectIceCream (personInventory));
	}
}
