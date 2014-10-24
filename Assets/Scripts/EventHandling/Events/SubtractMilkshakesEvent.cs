using UnityEngine;
using System.Collections;

public class SubtractMilkshakesEvent : GameEvent {

	public int amount;

	public SubtractMilkshakesEvent (int amount) {
		this.amount = amount;
	}
}
