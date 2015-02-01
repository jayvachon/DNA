using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class House : StaticUnit, IInventoryHolder, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public AcceptedActions AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HouseInventory ();
		AcceptedActions = new HouseAcceptedActions ();
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
}

public class HouseInventory : Inventory {

	public HouseInventory () {
		Add (new ElderHolder (2, 2));
	}
}

public class HouseAcceptedActions : AcceptedActions {

	public HouseAcceptedActions () {
		Add<CollectItem<ElderHolder>> ();
		Add<DeliverItem<ElderHolder>> ();
	}
}