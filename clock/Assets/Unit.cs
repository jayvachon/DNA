using UnityEngine;
using System.Collections;
using GameClock;

public class Unit : MonoBehaviour, ITimeable {

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Clock.instance.WaitForBeats (this, 2);
		}
	}

	public void OnBeatsElapsed () {
		Debug.Log ("heard");
	}
}
