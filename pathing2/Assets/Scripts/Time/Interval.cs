using UnityEngine;
using System.Collections;

public class Interval {

	public virtual void Begin (float length) {
		TimeManager.Instance.WaitForSeconds (length, Run, End);
	}

	public virtual void Run (float progress) {}
	public virtual void End () {}
}
