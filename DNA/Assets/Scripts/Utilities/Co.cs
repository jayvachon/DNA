using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// TODO: rename to Coroutine once current Coroutine class has been removed

public class Co : MonoBehaviour {

	bool running;
	float duration;
	System.Action<float> action;
	System.Action onEnd;
	Func<bool> condition;

	// TODO: add static functions for waitforseconds, waitforcondition, etc.

	public static Co Start (float duration, System.Action<float> action, System.Action onEnd=null, Func<bool> condition=null) {
		Co co = ObjectPool.Instantiate<Co> ();
		co.Begin (duration, action, onEnd, condition);
		return co;
	}

	void Begin (float duration, System.Action<float> action, System.Action onEnd, Func<bool> condition) {

		this.duration = duration;
		this.action = action;
		this.onEnd = onEnd;
		this.condition = condition;

		StartCoroutine (CoRun ());
	}

	public void Stop (bool triggerOnEnd=true) {
		if (!triggerOnEnd) onEnd = null;
		running = false;
	}

	void End () {
		if (onEnd != null) onEnd ();
		ObjectPool.Destroy (transform);
		running = false;
		duration = 0f;
		action = null;
		condition = null;
		onEnd = null;
	}

	IEnumerator CoRun () {
		
		float eTime = 0f;
		running = true;
	
		if (condition != null) {
			while (eTime < duration && running && condition ()) {
				eTime += Time.deltaTime;
				action (eTime / duration);
				yield return null;
			}
		} else {
			while (eTime < duration && running) {
				eTime += Time.deltaTime;
				action (eTime / duration);
				yield return null;
			}
		}

		End ();
	}
}