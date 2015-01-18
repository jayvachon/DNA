using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class DistributorUnit : MovableUnit, IInventoryHolder, IActionable {

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
		Add (new IceCreamHolder ());
		Add (new MilkHolder (5, 0));
		Add (new MilkshakeHolder ());
	}
}

public class DistributorActionsList : ActionsList {

	// TODO: this is broken because the unit it's bound to doesn't know which action to take
	// (it just does whatever action is first in the list)
	// a couple of approaches to fixing this problem:
	//		1. static unit tries to accept actions whose inventory is not empty
	//		2. movable unit (this) sets its activeAction when collecting & delivering items
	// 2nd approach might be better b/c the distributor should only be allowed to move 1 item 
	//	type at a time. It can set its activeAction when it's tasked w/ moving an item type.

	public DistributorActionsList (IActionable actionable, Inventory inventory) : base (actionable) {
		//Add (new CollectItem<IceCreamHolder> (inventory, 1, 2));
		//Add (new DeliverItem<IceCreamHolder> (inventory, -1, 2));
		Add (new CollectItem<MilkHolder> (inventory, 5, 3));
		Add (new DeliverItem<MilkHolder> (inventory, -1, 3));
		/*Add (new CollectItem<MilkshakeHolder> (inventory, 5, 1));
		Add (new DeliverItem<MilkshakeHolder> (inventory, -1, 1));*/
	}
}
