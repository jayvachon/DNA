using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MilkPoolUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public AcceptedActions AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new MilkPoolInventory ();
		ActionsList = new MilkPoolActionsList (this);
		AcceptedActions = new MilkPoolAcceptedActions ();
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
}

public class MilkPoolInventory : Inventory {

	public MilkHolder milkHolder;

	public MilkPoolInventory () {
		milkHolder = new MilkHolder (100, 100);
		Add (milkHolder);
	}
}

public class MilkPoolActionsList : ActionsList {

	public MilkPoolActionsList (IActionable actionable) : base (actionable) {
		
	}
}

public class MilkPoolAcceptedActions : AcceptedActions {

	public MilkPoolAcceptedActions () {
		Add<CollectItem<MilkHolder>> ();
	}
}