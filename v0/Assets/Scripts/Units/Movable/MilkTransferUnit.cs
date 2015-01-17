using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MilkTransferUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new MilkTranserInventory ();
		ActionsList = new MilkTransferActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class MilkTranserInventory : Inventory {

	public MilkTranserInventory () {
		Add (new MilkHolder (5, 0));
	}
}

public class MilkTransferActionsList : ActionsList {

	public MilkTransferActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<MilkHolder> (inventory, -1, 3));
		Add (new DeliverItem<MilkHolder> (inventory, -1, 3));
	}
}
