using UnityEngine;
using System.Collections;
using GameInventory;

public class Cow : StaticUnit, IInventoryHolder {
	
	public Inventory Inventory { get; set; }

	public override void OnAwake () {
		base.OnAwake ();
		Inventory = new Inventory ();
		Inventory.Add (new IceCreamHolder (10));
		MyActionsList = new CowActionsList ();
	}
}
