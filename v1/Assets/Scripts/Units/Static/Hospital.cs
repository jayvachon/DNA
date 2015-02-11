using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class Hospital : StaticUnit, IInventoryHolder, IActionAcceptor, IActionPerformer  {

	public Inventory Inventory { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }
	public PerformableActions PerformableActions { get; private set; }

	protected override void Awake () {
		base.Awake ();

		Inventory = new Inventory ();
		Inventory.Add (new ElderHolder (10, 0));
		Inventory.Add (new MilkHolder (10, 0));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> (new ElderCondition (false, true)));
		AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> (new ElderCondition (true, false)));
		AcceptableActions.Add ("DeliverMilk", new AcceptDeliverItem<MilkHolder> ());

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("ConsumeMilk", new ConsumeItem<MilkHolder> (5));

		InventoryDrawer.Create (MyTransform, Inventory);
	}
}