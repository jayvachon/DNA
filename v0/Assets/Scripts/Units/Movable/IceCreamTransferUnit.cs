using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class IceCreamTransferUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new IceCreamTransferInventory ();
		ActionsList = new IceCreamTransferActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class IceCreamTransferInventory : Inventory {

	public IceCreamTransferInventory () {
		Add (new IceCreamHolder (5));
	}
}

public class IceCreamTransferActionsList : ActionsList {

	public IceCreamTransferActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<IceCreamHolder> (inventory, 1, 2));
		Add (new DeliverItem<IceCreamHolder> (inventory, -1, 2));
	}
}
