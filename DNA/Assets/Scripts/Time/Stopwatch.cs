using UnityEngine;
using System.Collections;

public class Stopwatch {

	float startTime;

	public void Begin () {
		startTime = TimeManager.Instance.TimeSinceStart;
	}

	public float Stop () {

		// Duration
		return TimeManager.Instance.TimeSinceStart - startTime;
	}
}
