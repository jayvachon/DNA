using UnityEngine;
using System.Collections;

public class TempGUI : MonoBehaviour {

	ActionsList actionsList = null;
	Action[] actions;
	string[] actionNames;

	void Awake () {
		Events.instance.AddListener<ActivateActionsListEvent> (OnActivateActionsListEvent);
		Events.instance.AddListener<DeactivateActionsListEvent> (OnDeactivateActionsListEvent);
	}

	void OnGUI () {
		if (actionsList == null || actionNames.Length == 0) return;
		GUILayout.Label ("Directory");
		for (int i = 0; i < actionNames.Length; i ++) {
			if (GUILayout.Button (actionNames[i])) {
				actions[i].Perform ();
			}
		}
	}

	void OnActivateActionsListEvent (ActivateActionsListEvent e) {
		actionsList = e.actionsList;
		actions = e.actionsList.Actions;
		actionNames = e.actionsList.ActionNames;
	}

	void OnDeactivateActionsListEvent (DeactivateActionsListEvent e) {
		if (actionsList == e.actionsList) {
			actionsList = null;
		}
	}
}
