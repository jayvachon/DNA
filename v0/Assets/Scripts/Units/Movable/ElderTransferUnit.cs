using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class ElderTransferUnit : MovableUnit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new ElderTranserInventory ();
		ActionsList = new ElderTransferActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class ElderTranserInventory : Inventory {

	public ElderTranserInventory () {
		Add (new ElderHolder (5));
	}
}

public class ElderTransferActionsList : ActionsList {

	public ElderTransferActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<ElderHolder> (inventory, 1, 3));
		Add (new DeliverItem<ElderHolder> (inventory, -1, 3));
	}
}
