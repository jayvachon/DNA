using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	static List<System.Action<float>> coroutines = new List<System.Action<float>> ();

	public static void Start (float time, System.Action<float> action, System.Action endAction=null) {
		coroutines.Add (action);
		Coroutine.Instance.StartCoroutine (Coroutine.CoCoroutine (time, action, endAction));
	}

	public static void Stop (System.Action<float> action) {
		coroutines.Remove (action);
	}

	static IEnumerator CoCoroutine (float time, System.Action<float> action, System.Action endAction=null) {
		
		float eTime = 0f;
	
		while (eTime < time && coroutines.Contains (action)) {
			eTime += Time.deltaTime;
			action (eTime / time);			
			yield return null;
		}

		if (endAction != null) endAction ();
	}

	public static void WaitForFixedUpdate (System.Action action) {
		Coroutine.Instance.StartCoroutine (Coroutine.CoWaitForFixedUpdate (action));
	}

	static IEnumerator CoWaitForFixedUpdate (System.Action action) {
		yield return new WaitForFixedUpdate ();
		action ();
	}
}
