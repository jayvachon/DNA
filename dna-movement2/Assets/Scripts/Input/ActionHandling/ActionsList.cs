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
		set { activeAction = value; }
	}

	public int Count {
		get { return actions.Count; }
	}

	// TODO: currently, this is how ActionsLists build their lists
	// since this only ever happens on creation, there should just be a function to 
	// declare the list all at once (in the constructor)
	// .. this is important b/c should be encouraging list items to never be
	// added/removed, only enabling/disabling ("show"/"hide")
	public void Add (Action action) {
		actions.Add (action);
		if (actions.Count == 1) {
			activeAction = actions[0];
		}
	}
}
