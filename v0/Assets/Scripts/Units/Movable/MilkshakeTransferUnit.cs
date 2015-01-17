using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MilkshakeTransferUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new MilkshakeTranserInventory ();
		ActionsList = new MilkshakeTransferActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class MilkshakeTranserInventory : Inventory {

	public MilkshakeTranserInventory () {
		Add (new MilkshakeHolder (5));
	}
}

public class MilkshakeTransferActionsList : ActionsList {

	public MilkshakeTransferActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<MilkshakeHolder> (inventory, -1, 3));
		Add (new DeliverItem<MilkshakeHolder> (inventory, -1, 3));
	}
}