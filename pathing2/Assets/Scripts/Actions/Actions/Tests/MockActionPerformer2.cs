using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;

public class MockActionPerformer2 : MonoBehaviour, IInventoryHolder, IActionPerformer, IBinder {

	public GameObject acceptor1;
	public GameObject acceptor2;

	public Inventory Inventory { get; private set; }
	public PerformableActions PerformableActions { get; private set; }
	public IActionAcceptor BoundAcceptor { get; private set; }

	public IActionAcceptor acc1;
	public IActionAcceptor acc2;

	void Awake () {

		acc1 = acceptor1.GetScript<MockActionAcceptor> () as IActionAcceptor;
		acc2 = acceptor2.GetScript<MockActionAcceptor2> () as IActionAcceptor; 

		Inventory = new Inventory (this);
		Inventory.Add (new CoffeeHolder (10, 10));

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add (new CollectItem<CoffeeHolder> ());
	}

	void Start () {
		OnBindActionable (acc1);
	}

	public virtual void OnBindActionable (IActionAcceptor acceptor) {
		Bind ();
		BoundAcceptor = acceptor;
		ActionHandler.instance.Bind (this);
	}

	void Bind () {
		/*PerformableActions.PairActionsBetweenAcceptors (
			new List<IActionAcceptor> () { acc1, acc2 }
		);*/
	}

	public virtual void OnEndActions () {
		Debug.Log ("Performer 2: " + Inventory.Get<CoffeeHolder> ().Count);
	}
}
