using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class TransferUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new TransferInventory ();
		ActionsList = new TransferActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class TransferInventory : Inventory {

	public TransferInventory () {
		Add (new MilkshakeHolder (5));
		Add (new MilkHolder (5, 0));
		Add (new IceCreamHolder (5));
		Add (new ElderHolder (5));
	}
}

public class TransferActionsList : ActionsList {

	public TransferActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<MilkshakeHolder> (inventory, -1, 3));
		Add (new DeliverItem<MilkshakeHolder> (inventory, -1, 3));
		Add (new CollectItem<MilkHolder> (inventory, 5, 2));
		Add (new DeliverItem<MilkHolder> (inventory, -1, 2));
		Add (new CollectItem<IceCreamHolder> (inventory, 5, 2));
		Add (new DeliverItem<IceCreamHolder> (inventory, -1, 2));
		Add (new CollectItem<ElderHolder> (inventory, 1, 3));
		Add (new DeliverItem<ElderHolder> (inventory, -1, 3));
	}
}