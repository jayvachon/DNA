using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;

public class SetAcceptableActionsActiveStateTest : MonoBehaviour, IActionAcceptor {

	public AcceptableActions AcceptableActions { get; private set; }

	void Awake () {
		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add (new AcceptCollectItem<CoffeeHolder> ());
		AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
		PrintExpectedCount (2, "Active at start");
		AcceptableActions.DeactivateAll ();
		PrintExpectedCount (0, "None active after deactivate all");
		AcceptableActions.SetActive ("CollectCoffee", false);
		if (AcceptableActions.ActiveActions.ContainsKey ("CollectCoffee")) {
			Debug.Log ("deactivate single action failed");
		} else {
			Debug.Log ("deactivate single action succeeded");
		}
		AcceptableActions.SetActive ("CollectCoffee", true);
		if (AcceptableActions.ActiveActions.ContainsKey ("CollectCoffee")) {
			Debug.Log ("activate single action succeeded");
		} else {
			Debug.Log ("activate single action failed");
		}
	}

	bool IsCorrectActiveCount (int expectedCount) {
		return AcceptableActions.ActiveActions.Count == expectedCount;
	}

	void PrintExpectedCount (int expectedCount, string message) {
		if (IsCorrectActiveCount (expectedCount)) {
			Debug.Log (message + ": succeeded");
		} else {
			Debug.Log (message + ": failed");
		}
	}
}
