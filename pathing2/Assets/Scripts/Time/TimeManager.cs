using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	float time = 0;  // Seconds since start of game
	public float Time {
		get { return time; }
	}

	float scale = 1; // 0 to 1

	static TimeManager instance = null;
	public static TimeManager Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (TimeManager)) as TimeManager;
			}
			return instance;
		}
	}

	void Update () {
		//time += Time.deltaTime;
	}
}
