using UnityEngine;
using System.Collections;

public class TempGUI : MonoBehaviour {

	void OnGUI () {

		if (!ActionsListManager.HasActions)
			return;
		ActionsList actionsList = ActionsListManager.Actions;

		GUILayout.Label ("Directory");
		foreach (Action action in actionsList.Actions) {
			if (action.Enabled && action is IGUIActionable) {
				IGUIActionable guiActionable = action as IGUIActionable;
				if (GUILayout.Button (guiActionable.Label)) {
					actionsList.ActiveAction = action;
				}
			}
		}
	}
}
