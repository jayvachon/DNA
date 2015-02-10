using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using Pathing;

public class Distributor : MovableUnit, IInventoryHolder, IActionPerformer {

	public override string Name {
		get { return "Distributor"; }
	}

	public Inventory Inventory { get; set; }
	public PerformableActions PerformableActions { get; set; }

	protected override void Awake () {
		base.Awake ();
		Inventory = new Inventory ();
		PerformableActions = new PerformableActions (this);
		InventoryDrawer.Create (MyTransform, Inventory);
	}

	protected override void OnBindActionable (IActionAcceptor acceptor) {
		//PerformableActions.Start (acceptor);
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}