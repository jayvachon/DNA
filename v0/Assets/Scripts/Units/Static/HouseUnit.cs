﻿using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class HouseUnit : StaticUnit, IInventoryHolder, IActionAcceptor  {

	// Debugging
	public int elderCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HouseInventory ();
		AcceptedActions = new HouseAcceptedActions (Inventory as HouseInventory);
	}

	public void OnEndAction () {}

	/**
	 *	Debugging
	 */

	void Update () {
		elderCount = Inventory.Get<ElderHolder> ().Count;
	}
}

public class HouseInventory : Inventory {

	public HouseInventory () {
		Add (new ElderHolder (2, 2));
	}
}

public class HouseAcceptedActions : ActionsList {

	public HouseAcceptedActions (HouseInventory houseInventory) {
		Add (new CollectItem<ElderHolder> (houseInventory, 1, 3));
	}
}