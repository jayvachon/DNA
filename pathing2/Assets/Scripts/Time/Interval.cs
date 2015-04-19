using UnityEngine;
using System.Collections;

public delegate void OnRun (float progress);
public delegate void OnTimerEnd ();

public class Interval {

	protected OnRun onRun;
	protected OnTimerEnd onTimerEnd;

	public virtual void Begin (float length) {
		TimeManager.Instance.WaitForSeconds (length, Run, End);
	}

	public virtual void Run (float progress) {
		if (onRun != null) onRun (progress);
	}

	public virtual void End () {
		if (onTimerEnd != null) onTimerEnd ();
		onRun = null;
		onTimerEnd = null;
	}
}
