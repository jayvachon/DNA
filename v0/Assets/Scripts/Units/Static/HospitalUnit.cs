using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class HospitalUnit : StaticUnit, IInventoryHolder, IActionable, IActionAcceptor  {

	// Debugging
	public int iceCreamCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }
	public ActionsList AcceptedActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new HospitalInventory ();
		ActionsList = new HospitalActionsList (this);
		AcceptedActions = new HospitalAcceptedActionsList (Inventory as HospitalInventory);
	}

	public void OnEndAction () {}

	/**
	 *	Debugging
	 */

	void Update () {
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}

public class HospitalInventory : Inventory {

	public IceCreamHolder iceCreamHolder;

	public HospitalInventory () {
		iceCreamHolder = new IceCreamHolder (20);
		Add (iceCreamHolder);
	}
}

public class HospitalActionsList : ActionsList {

	public HospitalActionsList (IActionable actionable) : base (actionable) {
		
	}
}

public class HospitalAcceptedActionsList : ActionsList {

	public HospitalAcceptedActionsList (HospitalInventory hospitalInventory) {
		Add (new DeliverIceCream (hospitalInventory, 0));
	}
}
