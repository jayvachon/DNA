using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class House : StaticUnit, IInventoryHolder, IActionAcceptor  {

	public Inventory Inventory { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	protected override void Awake () {
		base.Awake ();

		Inventory = new Inventory ();
		Inventory.Add (new ElderHolder (2, 2));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> (new ElderCondition (true, true)));
		AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> (new ElderCondition (false, false)));

		InventoryDrawer.Create (UTransform, Inventory);
	}
}