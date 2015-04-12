using UnityEngine;
using System.Collections;

public class Coroutine : MonoBehaviour {

	static Coroutine instance = null;
	static public Coroutine Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (Coroutine)) as Coroutine;
				if (instance == null) {
					GameObject go = new GameObject ("Coroutine");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<Coroutine> ();
				}
			}
			return instance;
		}
	}

	public void StartCoroutine (float time, System.Action<float> action, System.Action endAction) {
		StartCoroutine (CoCoroutine (time, action, endAction));
	}

	public void StopCoroutine () {
		StopCoroutine ("CoCoroutine");
	}

	IEnumerator CoCoroutine (float time, System.Action<float> action, System.Action endAction) {
		
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			action (eTime / time);			
			yield return null;
		}

		if (endAction != null) endAction ();
	}
}
