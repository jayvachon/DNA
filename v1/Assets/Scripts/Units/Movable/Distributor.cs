using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using Pathing;

public class Distributor : MovableUnit, IInventoryHolder, IActionPerformer, IBinder {

	public override string Name {
		get { return "Distributor"; }
	}

	public Inventory Inventory { get; private set; }
	public PerformableActions PerformableActions { get; private set; }
	public IActionAcceptor BoundAcceptor { get; private set; }

	protected override void Awake () {
		base.Awake ();
		
		Inventory = new Inventory ();
		Inventory.Add (new ElderHolder (2, 0));
		Inventory.Add (new MilkHolder (5, 0));

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (2));
		PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (2));
		PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (0.5f));
		PerformableActions.Add ("DeliverMilk", new DeliverItem<MilkHolder> (0.5f));
		
		InventoryDrawer.Create (MyTransform, Inventory);
		PerformableActions.RefreshEnabledActions ();
		PerformableActions.SetDrawer (ActionDrawer.Create (MyTransform, PerformableActions.EnabledActionsList));
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		BoundAcceptor = acceptor;
		ActionHandler.instance.Bind (this);
	}

	public void OnEndActions () {
		StartMoveOnPath ();
	}
}