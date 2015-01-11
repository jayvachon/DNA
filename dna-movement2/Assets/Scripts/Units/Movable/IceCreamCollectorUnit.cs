using UnityEngine;
using System.Collections;
using GameInventory;

public class IceCreamCollectorUnit : MovableUnit, IInventoryHolder {

	public Inventory Inventory { get; set; }

	public override void OnAwake () {
		base.OnAwake ();
		Inventory.Add (new IceCreamHolder (5));
	}
}
