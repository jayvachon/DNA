using UnityEngine;
using System.Collections;

public enum ActionState {
	Idle, RoadConstruction, BuildingConstruction
}

public static class PlayerActionState {

	static ActionState state = ActionState.Idle;
	public static ActionState State {
		get { return state; }
	}

	public delegate void OnChange (ActionState state);

	public static OnChange onChange;

	public static void Set (ActionState newState) {
		state = newState;
		if (onChange != null)
			onChange (state);
	}
}
