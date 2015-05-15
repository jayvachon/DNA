using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class MockActionAcceptor2 : MonoBehaviour, IInventoryHolder, IActionAcceptor {

	public Inventory Inventory { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	void Awake () {

		Inventory = new Inventory (this);
		Inventory.Add (new CoffeeHolder (10, 10));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
	}
}
