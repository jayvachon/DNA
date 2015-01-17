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

	public IceCreamHolder iceCreamHolder;

	public HospitalInventory () {
		iceCreamHolder = new IceCreamHolder (20);
		Add (iceCreamHolder);
		Add (new ElderHolder (10));
	}
}

public class HospitalActionsList : ActionsList {

	public HospitalActionsList (IActionable actionable) : base (actionable) {
		
	}
}

public class HospitalAcceptedActions : AcceptedActions {

	public HospitalAcceptedActions () {
		Add<DeliverItem<IceCreamHolder>> ();
		Add<DeliverItem<ElderHolder>> ();
	}
}
