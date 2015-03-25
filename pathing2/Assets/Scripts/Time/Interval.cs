using UnityEngine;
using System.Collections;

public class Interval : MonoBehaviour {

	float length = 0;
	float startTime = 0;

	public void Begin () {
		startTime = TimeManager.Instance.Time;
	}

	
}
