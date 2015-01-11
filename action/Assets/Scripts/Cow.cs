using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class Cow : Unit, IInventoryHolder, IActionable {

	public int iceCreamCount = 0;

	public Inventory Inventory { get; set; }
	public ActionsList ActionsList { get; set; }

	void Awake () {
		
		IceCreamHolder iceCreamHolder = new IceCreamHolder (10);
		Inventory = new Inventory ();
		Inventory.Add (iceCreamHolder);

		ActionsList = new ActionsList ();
		ActionsList.Add (new GenerateIceCream (iceCreamHolder));
	}

	void Update () {
		iceCreamCount = Inventory.Get<IceCreamHolder> ().Count;
	}
}
