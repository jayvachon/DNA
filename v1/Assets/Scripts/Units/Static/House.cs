using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class House : StaticUnit, IInventoryHolder, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public AcceptableActions AcceptableActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new Inventory ();
		AcceptableActions = new AcceptableActions (this);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
}