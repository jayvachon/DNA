using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class HospitalUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public ActionsList AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HospitalInventory ();
		ActionsList = new HospitalActionsList (this);
		AcceptedActions = new HospitalAcceptedActionsList ();
	}

	public void OnEndAction () {}
}

public class HospitalInventory : Inventory {

	public IceCreamHolder iceCreamHolder;

	public HospitalInventory () {
		iceCreamHolder = new IceCreamHolder (10);
		Add (iceCreamHolder);
	}
}

public class HospitalActionsList : ActionsList {

	public HospitalActionsList (IActionable actionable) : base (actionable) {
		
	}
}

public class HospitalAcceptedActionsList : ActionsList {

	public HospitalAcceptedActionsList () {
		
	}
}
