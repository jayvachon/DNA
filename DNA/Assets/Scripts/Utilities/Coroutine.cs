using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// TODO: deprecate this - use Co class instead

public class Coroutine : MonoBehaviour {

	static Coroutine instance = null;
	static public Coroutine Instance {
		get {
			if (instance == null) {
				instance = UnityEngine.Object.FindObjectOfType (typeof (Coroutine)) as Coroutine;
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

	public static void StartWithCondition (float time, System.Action<float> action, Func<bool> condition, System.Action endAction=null) {
		coroutines.Add (action);
		Coroutine.Instance.StartCoroutine (Coroutine.CoCoroutineWithCondition (time, action, condition, endAction));
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

		coroutines.Remove (action);
		if (endAction != null) endAction ();
	}

	static IEnumerator CoCoroutineWithCondition (float time, System.Action<float> action, Func<bool> condition, System.Action endAction=null) {

		float eTime = 0f;

		while (eTime < time && coroutines.Contains (action) && condition ()) {
			eTime += Time.deltaTime;
			action (eTime / time);
			yield return null;
		}

		coroutines.Remove (action);
		if (endAction != null) endAction ();
	}

	public static void WaitForCondition (Func<bool> condition, System.Action onEnd) {
		Coroutine.Instance.StartCoroutine (Coroutine.CoWaitForCondition (condition, onEnd));
	}

	static IEnumerator CoWaitForCondition (Func<bool> condition, System.Action onEnd) {
		while (!condition ()) yield return null;
		onEnd ();
	}

	public static void WaitForSeconds (float time, System.Action onEnd) {
		Coroutine.Instance.StartCoroutine (Coroutine.CoWaitForSeconds (time, onEnd));
	}

	static IEnumerator CoWaitForSeconds (float time, System.Action onEnd) {
		yield return new WaitForSeconds (time);
		onEnd ();
	}

	public static void WaitForFixedUpdate (System.Action action) {
		Coroutine.Instance.StartCoroutine (Coroutine.CoWaitForFixedUpdate (action));
	}

	static IEnumerator CoWaitForFixedUpdate (System.Action action) {
		yield return new WaitForFixedUpdate ();
		action ();
	}
}