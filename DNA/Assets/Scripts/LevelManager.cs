using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	void Awake () {
		LevelStart ();
		Sea.Instance.endRising += OnEndRising;
	}

	void LevelStart () {
		EmissionsManager.Reset ();
		Sea.Instance.BeginRising ();
	}

	void OnEndRising () {
		// game over
		Debug.Log ("game over");
	}
}
