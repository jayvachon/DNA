using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// TODO: rename to Coroutine once current Coroutine class has been removed

public class Co : MonoBehaviour {

	bool running;
	float time;
	System.Action<float> action;
	System.Action onEnd;
	Func<bool> condition;

	// TODO: add static functions for waitforseconds, waitforcondition, etc.

	public static Co Start (float time, System.Action<float> action, System.Action onEnd=null, Func<bool> condition=null) {
		Co co = ObjectPool.Instantiate<Co> ();
		co.Begin (time, action, onEnd, condition);
		return co;
	}

	public void Stop (bool triggerOnEnd=true) {
		if (!triggerOnEnd) onEnd = null;
		End ();
	}

	void Begin (float time, System.Action<float> action, System.Action onEnd, Func<bool> condition) {

		this.time = time;
		this.action = action;
		this.onEnd = onEnd;
		this.condition = condition;

		running = true;
		StartCoroutine (CoStart ());
	}

	void End () {
		if (onEnd != null) onEnd ();
		running = false;
		ObjectPool.Destroy (transform);
	}

	void OnDisable () {
		running = false;
		time = 0f;
		action = null;
		onEnd = null;
		condition = null;
	}

	IEnumerator CoStart () {
		
		float eTime = 0f;
	
		if (condition != null) {
			while (eTime < time && running && condition ()) {
				eTime += Time.deltaTime;
				action (eTime / time);
				yield return null;
			}
		} else {
			while (eTime < time && running) {
				eTime += Time.deltaTime;
				action (eTime / time);
				yield return null;
			}
		}

		End ();
	}
}