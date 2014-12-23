using UnityEngine;
using System.Collections;

public class TempGUI : MonoBehaviour {

	void OnGUI () {

		if (!ActionsListManager.HasActions)
			return;
		ActionsList actions = ActionsListManager.Actions;

		GUILayout.Label ("Directory");
		string[] names = actions.ActionNames;
		for (int i = 0; i < names.Length; i ++) {
			string name = names[i];
			if (GUILayout.Button (name)) {
				actions.SetActiveAction (name);
			}
		}
	}
}
