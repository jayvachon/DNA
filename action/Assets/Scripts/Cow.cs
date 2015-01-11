using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class Cow : Unit, IInventoryHolder, IActionable {

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	void Awake () {
		
		IceCreamHolder iceCreamHolder = new IceCreamHolder (5);
		Inventory = new Inventory ();
		Inventory.Add (iceCreamHolder);

		ActionsList = new ActionsList ();
		ActionsList.Add (new GenerateIceCream (iceCreamHolder));
	}
}
