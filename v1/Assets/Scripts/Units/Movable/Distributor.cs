using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using Pathing;

public class Distributor : MovableUnit, IInventoryHolder, IActionable {

	public override string Name {
		get { return "Distributor"; }
	}

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new DistributorInventory ();
		ActionsList = new DistributorActionsList (this, Inventory);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}

public class DistributorInventory : Inventory {

	public DistributorInventory () {
		Add (new ElderHolder (3, 0));
	}
}

public class DistributorActionsList : ActionsList {

	public DistributorActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		Add (new CollectItem<ElderHolder> (inventory, 1, 3));
		Add (new DeliverItem<ElderHolder> (inventory, 1, 3));
	}
}