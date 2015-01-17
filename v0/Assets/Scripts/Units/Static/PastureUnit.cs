using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class PastureUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public AcceptedActions AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new PastureInventory ();
		ActionsList = new PastureActionsList (this, Inventory as PastureInventory);
		AcceptedActions = new PastureAcceptedActions ();
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
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

public class PastureAcceptedActions : AcceptedActions {

	public PastureAcceptedActions () {
		Add<CollectItem<IceCreamHolder>> ();
	}
}