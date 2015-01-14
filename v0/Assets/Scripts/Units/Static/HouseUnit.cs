using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class HouseUnit : StaticUnit, IInventoryHolder, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HouseInventory ();
		AcceptedActions = new HouseAcceptedActions (Inventory as HouseInventory);
	}

	public void OnEndAction () {}
}

public class HouseInventory : Inventory {

	public HouseInventory () {

	}
}

public class HouseAcceptedActions : ActionsList {

	public HouseAcceptedActions (HouseInventory houseInventory) {

	}
}