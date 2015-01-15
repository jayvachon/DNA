using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class PastureUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	// Debugging
	public int iceCreamCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public ActionsList AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new PastureInventory ();
		ActionsList = new PastureActionsList (this, Inventory as PastureInventory);
		AcceptedActions = new PastureAcceptedActions (Inventory);
	}

	public void OnEndAction () {}

	/**
	 *	Debugging
	 */

	void Update () {
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}

public class PastureInventory : Inventory {

	public IceCreamHolder iceCreamHolder;

	public PastureInventory () {
		iceCreamHolder = new IceCreamHolder (10);
		Add (iceCreamHolder);
	}
}

public class PastureActionsList : ActionsList {

	public PastureActionsList (IActionable actionable, PastureInventory pastureInventory) : base (actionable) {
		Add (new GenerateIceCream (pastureInventory.iceCreamHolder));
	}
}

public class PastureAcceptedActions : ActionsList {

	public PastureAcceptedActions (Inventory inventory) {
		Add (new CollectItem<IceCreamHolder> (inventory, 0, 2));
	}
}