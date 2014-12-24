using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionsList {

	List<Action> actions = new List<Action> ();
	public List<Action> Actions {
		get { return actions; }
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

	public void SetActiveAction (Action action) {
		activeAction = action;
	}
}
