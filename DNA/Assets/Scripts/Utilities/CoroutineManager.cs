using UnityEngine;
using System;
using System.Collections;

public class CoroutineManager : MonoBehaviour {

	static CoroutineManager instance = null;
	static public CoroutineManager Instance {
		get {
			if (instance == null) {
				instance = UnityEngine.Object.FindObjectOfType (typeof (CoroutineManager)) as CoroutineManager;
				if (instance == null) {
					GameObject go = new GameObject ("CoroutineManager");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<CoroutineManager> ();
				}
			}
			return instance;
		}
	}
}
