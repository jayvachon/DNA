using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;

public class MockActionPerformer : MonoBehaviour, IInventoryHolder, IActionPerformer, IBinder {

	public bool testPairing = false;
	public bool testEnabling = true;

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
		Inventory.Add (new MilkshakeHolder (10, 10));
		Inventory.Add (new HappinessHolder (5, 0));

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add (new CollectItem<CoffeeHolder> ());
		PerformableActions.Add (new DeliverItem<CoffeeHolder> ());
		PerformableActions.Add (new DeliverItem<MilkshakeHolder> ());
		// PerformableActions.Add (new ConsumeItem<HappinessHolder> ());
		// PerformableActions.Add (new GenerateItem<HappinessHolder> ());
	}

	void Start () {
		if (testEnabling) {
			
			// Inventory.Get<CoffeeHolder> ().Clear ();
			// OnBindActionable (acc1);
			// Debug.Log ("don't enable if inventory empty: " + PerformableActions.Get ("CollectCoffee").Enabled);
			// Inventory.AddItem<CoffeeHolder> (new CoffeeItem ());
			
			// IInventoryHolder acceptorInventory = acc1 as IInventoryHolder;
			// acceptorInventory.Inventory.Get<CoffeeHolder> ().Clear ();
			// OnBindActionable (acc1);

			// OnBindActionable (acc1);

		} else {
			OnBindActionable (acc1);
		}
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
		PrintPaired ();
	}

	public virtual void OnEndActions () {
		// Debug.Log ("Performer 1: " + Inventory.Get<CoffeeHolder> ().Count);
		Debug.Log ("Performer 1: " + Inventory.Get<HappinessHolder> ().Count);
	}

	void PrintPaired () {
		if (!testPairing) return;
		foreach (var action in PerformableActions.ActiveActions) {
			string name = action.Key;
			EnabledState es = action.Value.EnabledState;
			if (es.RequiredPair == "") {
				Debug.Log (action.Key + " does not require a pair");
			} else if (es.Paired) {
				Debug.Log (name + " is paired");
			} else {
				Debug.Log (name + " is unpaired");
			}
		}
	}

	void PrintEnabled () {
		if (!testEnabling) return;
		foreach (var action in PerformableActions.ActiveActions) {
			string name = action.Key;
			EnabledState es = action.Value.EnabledState;
			if (es.Enabled) {
				Debug.Log (name + " is enabled");
			} else {
				Debug.Log (name + " is disabled");
			}
		}
	}
}
