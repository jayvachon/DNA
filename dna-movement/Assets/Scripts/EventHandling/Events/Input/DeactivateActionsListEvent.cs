using UnityEngine;
using System.Collections;

public class DeactivateActionsListEvent : GameEvent {

	public readonly ActionsList actionsList;

	public DeactivateActionsListEvent (ActionsList actionsList) {
		this.actionsList = actionsList;
	}
}
