using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class IceCreamCollectorUnit : MovableUnit, IInventoryHolder, IActionable {

	// Debugging
	public int iceCreamCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new IceCreamCollectorInventory ();
		ActionsList = new IceCreamCollectorActionsList (this, Inventory as IceCreamCollectorInventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		ActionsList.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}

	/**
	 *	Debugging
	 */

	protected override void Update () {
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
		base.Update ();
	}
}

public class IceCreamCollectorInventory : Inventory {

	public IceCreamCollectorInventory () {
		Add (new IceCreamHolder (5));
	}
}

public class IceCreamCollectorActionsList : ActionsList {

	public IceCreamCollectorActionsList (IActionable actionable, IceCreamCollectorInventory iceCreamCollectorInventory) : base (actionable) {
		Add (new CollectIceCream (iceCreamCollectorInventory, 1));
		Add (new DeliverIceCream (iceCreamCollectorInventory, -1));
	}
}
