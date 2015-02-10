using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;

public class MovableUnit : MonoBehaviour, IInventoryHolder, IActionPerformer, IBinder {

	// Debugging
	public StaticUnit boundUnit;

	public Inventory Inventory { get; private set; }
	public PerformableActions PerformableActions { get; private set; }
	public IActionAcceptor BoundAcceptor { get; private set; }

	void Awake () {

		Inventory = new Inventory ();
		Inventory.Add (new ElderHolder (5, 2));

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (3));
		//PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (3));
		PerformableActions.Enable ("DeliverElder");
		PerformableActions.Enable ("CollectElder");

		// Debugging
		BoundAcceptor = boundUnit as IActionAcceptor;
		/*Debug.Log("Movable Unit:");
		PerformableActions.Print ();*/
	}

	void Start () {
		ActionHandler.instance.Bind (this);
	}

	public void OnEndActions () {
		Inventory.Print ();
	}
}
