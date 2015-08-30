using UnityEngine;
using System.Collections;

public class AddMilkshakesEvent : GameEvent {

	public int amount;

	public AddMilkshakesEvent (int amount) {
		this.amount = amount;
	}
}
