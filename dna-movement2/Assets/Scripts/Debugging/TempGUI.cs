using UnityEngine;
using System.Collections;

public class TempGUI : MonoBehaviour {

	void OnGUI () {

		if (ActionsListManager.instance == null)
			return;
		if (ActionsListManager.instance.Actions == null)
			return;

		ActionsList actions = ActionsListManager.instance.Actions;
		if (actions.Count == 0) 
			return;
			
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
