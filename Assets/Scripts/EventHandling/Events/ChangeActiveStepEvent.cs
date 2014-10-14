using UnityEngine;
using System.Collections;

public class ChangeActiveStepEvent : GameEvent {

	public Step step;

	public ChangeActiveStepEvent (Step _step) {
		step = _step;
	}
}
