using UnityEngine;
using System.Collections;

public static class ActionsListManager {

	static ActionsList actionsList = null;
	public static ActionsList Actions {
		get { return actionsList; }
		set {
			actionsList = value;
		}
	}

	public static bool HasActions {
		get { return actionsList != null && actionsList.Count > 0; }
	}
}
