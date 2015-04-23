using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Sea sea;

	void Awake () {
		LevelStart ();
		sea.endRising += OnEndRising;
	}

	void LevelStart () {
		sea.BeginRising ();
	}

	void OnEndRising () {
		// game over
		Debug.Log ("game over");
	}
}
