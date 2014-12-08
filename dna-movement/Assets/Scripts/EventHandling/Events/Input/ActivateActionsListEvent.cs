using UnityEngine;
using System.Collections;

public class ActivateActionsListEvent : GameEvent {

	public readonly ActionsList actionsList;

	public ActivateActionsListEvent (ActionsList actionsList) {
		this.actionsList = actionsList;
	}
}
