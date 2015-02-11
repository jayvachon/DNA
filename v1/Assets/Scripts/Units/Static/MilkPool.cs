using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MilkPool : StaticUnit, IInventoryHolder, IActionAcceptor  {

	public Inventory Inventory { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	protected override void Awake () {
		base.Awake ();

		Inventory = new Inventory ();
		Inventory.Add (new MilkHolder (100, 100));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add ("CollectMilk", new AcceptCollectItem<MilkHolder> ());

		InventoryDrawer.Create (MyTransform, Inventory);
	}
}
