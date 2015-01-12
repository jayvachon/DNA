using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class IceCreamCollectorUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new IceCreamCollectorInventory ();
		ActionsList = new IceCreamCollectorActionsList (Inventory as IceCreamCollectorInventory);
	}
}

public class IceCreamCollectorInventory : Inventory {

	public IceCreamCollectorInventory () {
		Add (new IceCreamHolder (5));
	}
}

public class IceCreamCollectorActionsList : ActionsList {

	public IceCreamCollectorActionsList (IceCreamCollectorInventory iceCreamCollectorInventory) {
		Add (new CollectIceCream (iceCreamCollectorInventory));
	}
}
