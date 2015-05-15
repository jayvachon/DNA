using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MockActionAcceptor : MonoBehaviour, IInventoryHolder, IActionAcceptor {

	public Inventory Inventory { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	void Awake () {

		Inventory = new Inventory (this);
		Inventory.Add (new CoffeeHolder (5, 0));
		Inventory.Add (new MilkshakeHolder (10, 0));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
		AcceptableActions.Add (new AcceptDeliverItem<MilkshakeHolder> ());
	}

	void PrintEnabled () {
		foreach (var action in AcceptableActions.ActiveActions) {
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
