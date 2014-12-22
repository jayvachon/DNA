using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionsList {

	List<Action> actions = new List<Action> ();
	public List<Action> Actions {
		get { return actions; }
	}

	string[] actionNames = new string[0];
	public string[] ActionNames {
		get {
			if (actionNames.Length == 0) {
				actionNames = new string[Count];
				for (int i = 0; i < Count; i ++) {
					actionNames[i] = actions[i].name;
				}
			}
			return actionNames;
		}
	}

	Action activeAction = null;
	public Action ActiveAction {
		get { return activeAction; }
	}

	public int Count {
		get { return actions.Count; }
	}

	public void Add (Action action) {
		actions.Add (action);
		if (actions.Count == 1) {
			activeAction = actions[0];
		}
	}

	public void SetActiveAction (string name) {
		Action a = GetAction (name);
		if (a.CanSetActive ()) {
			activeAction = GetAction (name);
		}
	}

	Action GetAction (string name) {
		foreach (Action a in actions) {
			if (a.name == name)
				return a;
		}
		return null;
	}
}
