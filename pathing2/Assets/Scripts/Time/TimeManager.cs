using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	float time = 0;  // Seconds since start of game
	public float TimeSinceStart {
		get { return time; }
	}

	//float scale = 1; // 0 to 1

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
		time += Time.deltaTime;
	}

	public void WaitForSeconds (float duration, System.Action action) {
		StartCoroutine (CoWaitForSeconds (duration, action));
	}

	IEnumerator CoWaitForSeconds (float duration, System.Action action) {
		
		float startTime = time;
		float endTime = startTime + duration;

		while (time < endTime) {
			yield return null;
		}

		action ();
	}
}
