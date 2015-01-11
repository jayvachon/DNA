using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class Cow : Unit, IInventoryHolder, IActionable {

	public int iceCreamCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	void Awake () {
		Inventory = new CowInventory ();
		ActionsList = new CowActionsList (Inventory as CowInventory);
	}

	void Update () {
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}

public class CowInventory : Inventory {

	public IceCreamHolder iceCreamHolder;

	public CowInventory () {
		iceCreamHolder = new IceCreamHolder (10);
		Add (iceCreamHolder);
	}
}

public class CowActionsList : ActionsList {

	public CowActionsList (CowInventory cowInventory) {
		Add (new GenerateIceCream (cowInventory.iceCreamHolder));
	}
}
