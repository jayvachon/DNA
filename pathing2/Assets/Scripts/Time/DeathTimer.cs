using UnityEngine;
using System.Collections;

public class DeathTimer : Interval {

	float deathAge = TimerValues.Death;

	public void BeginAging () {
		Begin (deathAge);
	}
}
