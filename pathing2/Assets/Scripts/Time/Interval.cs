using UnityEngine;
using System.Collections;

public class Interval {

	public virtual void Begin (float length) {
		TimeManager.Instance.WaitForSeconds (length, End);
	}

	public virtual void End () {}
}
