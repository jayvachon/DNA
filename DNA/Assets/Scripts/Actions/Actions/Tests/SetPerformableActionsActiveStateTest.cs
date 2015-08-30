using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;

public class SetPerformableActionsActiveStateTest : MonoBehaviour, IActionPerformer {

	public PerformableActions PerformableActions { get; private set; }

	void Awake () {
		PerformableActions = new PerformableActions (this);
		PerformableActions.Add (new CollectItem<CoffeeHolder> ());
		PerformableActions.Add (new DeliverItem<CoffeeHolder> ());
		PrintExpectedCount (2, "Active at start");
		PerformableActions.DeactivateAll ();
		PrintExpectedCount (0, "None active after deactivate all");
		PerformableActions.SetActive ("CollectCoffee", false);
		if (PerformableActions.ActiveActions.ContainsKey ("CollectCoffee")) {
			Debug.Log ("deactivate single action failed");
		} else {
			Debug.Log ("deactivate single action succeeded");
		}
		PerformableActions.SetActive ("CollectCoffee", true);
		if (PerformableActions.ActiveActions.ContainsKey ("CollectCoffee")) {
			Debug.Log ("activate single action succeeded");
		} else {
			Debug.Log ("activate single action failed");
		}
	}

	bool IsCorrectActiveCount (int expectedCount) {
		return PerformableActions.ActiveActions.Count == expectedCount;
	}

	void PrintExpectedCount (int expectedCount, string message) {
		if (IsCorrectActiveCount (expectedCount)) {
			Debug.Log (message + ": succeeded");
		} else {
			Debug.Log (message + ": failed");
		}
	}
}
