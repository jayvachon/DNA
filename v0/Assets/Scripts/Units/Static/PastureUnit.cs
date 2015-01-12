using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class PastureUnit : StaticUnit, IInventoryHolder, IActionable  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new PastureInventory ();
		ActionsList = new PastureActionsList (Inventory as PastureInventory);
	}
}

public class PastureInventory : Inventory {

	public IceCreamHolder iceCreamHolder;

	public PastureInventory () {
		iceCreamHolder = new IceCreamHolder (10);
		Add (iceCreamHolder);
	}
}

public class PastureActionsList : ActionsList {

	public PastureActionsList (PastureInventory pastureInventory) {
		Add (new GenerateIceCream (pastureInventory.iceCreamHolder));
	}
}
