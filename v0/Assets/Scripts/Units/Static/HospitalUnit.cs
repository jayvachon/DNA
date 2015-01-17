using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class HospitalUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public AcceptedActions AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HospitalInventory ();
		ActionsList = new HospitalActionsList (this);
		AcceptedActions = new HospitalAcceptedActions ();
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	public void OnEndAction () {}
}

public class HospitalInventory : Inventory {

	public HospitalInventory () {
		Add (new MilkshakeHolder (20));
		Add (new ElderHolder (10));
	}
}

public class HospitalActionsList : ActionsList {

	public HospitalActionsList (IActionable actionable) : base (actionable) {
		
	}
}

public class HospitalAcceptedActions : AcceptedActions {

	public HospitalAcceptedActions () {
		Add<DeliverItem<MilkshakeHolder>> ();
		Add<DeliverItem<ElderHolder>> ();
	}
}
