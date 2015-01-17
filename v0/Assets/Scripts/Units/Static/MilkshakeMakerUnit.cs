using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MilkshakeMakerUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public AcceptedActions AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new MilkshakeMakerInventory ();
		ActionsList = new MilkshakeMakerActionsList (this, Inventory as MilkshakeMakerInventory);
		AcceptedActions = new MilkshakeMakerAcceptedActions ();
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
}

public class MilkshakeMakerInventory : Inventory {

	public IceCreamHolder iceCreamHolder = new IceCreamHolder (10);
	public MilkHolder milkHolder = new MilkHolder (10, 0);
	public MilkshakeHolder milkshakeHolder = new MilkshakeHolder (100);

	public MilkshakeMakerInventory () {
		Add (iceCreamHolder);
		Add (milkHolder);
		Add (milkshakeHolder);
	}
}

public class MilkshakeMakerActionsList : ActionsList {

	public MilkshakeMakerActionsList (IActionable actionable, MilkshakeMakerInventory inventory) : base (actionable) {
		Add (new MakeMilkshake (inventory.iceCreamHolder, inventory.milkHolder, inventory.milkshakeHolder));
	}
}

public class MilkshakeMakerAcceptedActions : AcceptedActions {

	public MilkshakeMakerAcceptedActions () {
		Add<DeliverItem<MilkHolder>> ();
		Add<DeliverItem<IceCreamHolder>> ();
		Add<CollectItem<MilkshakeHolder>> ();
	}
}