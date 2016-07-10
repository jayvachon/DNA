using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Utility class for running coroutines. Allows coroutines to be run in classes not derived from MonoBehaviour. Uses anonymous functions.
/// </summary>
public static class Co2 {

	static List<Action<float>> actions = new List<Action<float>> ();

	/// <summary>
	/// Runs a coroutine
	/// </summary>
	/// <param name="func">The coroutine to run</param>
	public static void StartCoroutine (Func<IEnumerator> func) {
		CoMb.Instance.StartCoroutine (func ());
	}

	/// <summary>
	/// Waits for an amount of time, and then runs a function
	/// </summary>
	/// <param name="seconds">The amount of time to wait</param>
	/// <param name="onEnd">The function to run</param>
	public static void WaitForSeconds (float seconds, Action onEnd) {
		CoMb.Instance.StartCoroutine (CoWaitForSeconds (seconds, onEnd));
	}

	/// <summary>
	/// Waits one frame, and then runs a function
	/// </summary>
	/// <param name="onEnd">The function to run</param>
	public static void WaitForFixedUpdate (Action onEnd) {
		CoMb.Instance.StartCoroutine (CoWaitForFixedUpdate (onEnd));
	}

	/// <summary>
	/// Runs a function every frame for an amount of time
	/// </summary>
	/// <param name="duration">The amount of time to run (in seconds)</param>
	/// <param name="action">The function to call every frame. Passes the % of time that has elapsed as a float</param>
	/// <param name="onEnd">(option) A function to run after the time has elapsed</param>
	public static void StartCoroutine (float duration, Action<float> action, Action onEnd=null) {
		actions.Add (action);
		CoMb.Instance.StartCoroutine (CoCoroutine (duration, action, onEnd));
	}

	//// <summary>
	/// Stops a coroutine immediately
	/// </summary>
	//// <param name="action">The action reference of the coroutine to stop</param>
	public static void StopCoroutine (Action<float> action) {
		actions.Remove (action);
	}

	/// <summary>
	/// Waits until an expression evaluates as false, and then calls a function
	/// </summary>
	/// <param name="condition">The expression to evaluate. Continues waiting as long as the expression returns true.</param>
	/// <param name="onEnd">The function to call when the expression returns false</param>
	public static void YieldWhileTrue (Func<bool> condition, Action onEnd) {
		CoMb.Instance.StartCoroutine (CoYieldWhileTrue (condition, onEnd));
	} 

	/// <summary>
	/// Runs a function every frame as long as the condition evaluates to true
	/// </summary>
	/// <param name="condition">The expression to evaluate</param>
	/// <param name="onRun">The function to run while true</param>
	/// <param name="onEnd">(optional) a function to run when the loop exits</param>
	public static void RunWhileTrue (Func<bool> condition, Action onRun, Action onEnd=null) {
		CoMb.Instance.StartCoroutine (CoRunWhileTrue (condition, onRun, onEnd));
	}

	/// <summary>
	/// Repeatedly invokes a function as long as the condition is met
	/// </summary>
	/// <param name="time">(optional) The initial delay before invoking begins</param>
	/// <param name="rate">The delay between invoke calls</param>
	/// <param name="condition">The expression to evaluate. When 'condition' is false, the coroutine stops.</param>
	/// <param name="onEnd">(optional) A function to run after the coroutine has finished</param>
	public static void InvokeWhileTrue (float time, float rate, Func<bool> condition, Action onInvoke, Action onEnd=null) {
		
		float duration = time > 0f ? time : rate;

		Co2.WaitForSeconds (duration, () => {
			if (condition ()) {
				onInvoke ();
				InvokeWhileTrue (0f, rate, condition, onInvoke, onEnd);
			} else if (onEnd != null) {
				onEnd ();
			}
		});
	}

	public static void InvokeWhileTrue (float rate, Func<bool> condition, Action onInvoke, Action onEnd=null) {
		InvokeWhileTrue (0f, rate, condition, onInvoke, onEnd);
	}

	/// <summary>
	/// Repeats an action a given number of times (like a 'for' loop with a delay between each iteration). Counts down to zero.
	/// </summary>
	/// <seeAlso cref="RepeatAscending" />
	/// <param name="time">(optional) The initial delay before counting begins</param>
	/// <param name="rate">The delay between actions</param>
	/// <param name="count">The number of times to repeat</param>
	/// <param name="onInvoke">The function to call. Passes in the loop index.</param>
	/// <param name="onEnd">(optional) The function to call when the count is 0</param>
	public static void Repeat (float time, float rate, int count, Action<int> onInvoke, Action onEnd=null) {
		InvokeWhileTrue (time, rate, () => { return count > 0; }, () => {
			count --;
			onInvoke (count);
			if (count == 0 && onEnd != null)
				onEnd ();
		});
	}

	public static void Repeat (float rate, int count, Action<int> onInvoke, Action onEnd=null) {
		Repeat (0f, rate, count, onInvoke, onEnd);
	}

	/// <summary>
	/// Repeats an action indefinitely
	/// </summary>
	/// <seeAlso cref="Repeat" />
	/// <param name="rate">The delay between actions</param>
	/// <param name="onInvoke">The function to call</param>
	public static void Repeat (float rate, Action onInvoke) {
		InvokeWhileTrue (0f, rate, () => { return Application.isPlaying; }, () => {
			onInvoke ();
		});
	}

	/// <summary>
	/// Repeats an action a given number of times (like a 'for' loop with a delay between each iteration). Counts up from zero.
	/// </summary>
	/// <seeAlso cref="Repeat" />
	/// <param name="time">(optional) The initial delay before counting begins</param>
	/// <param name="rate">The delay between actions</param>
	/// <param name="count">The number of times to repeat</param>
	/// <param name="onInvoke">The function to call. Passes in the loop index.</param>
	/// <param name="onEnd">(optional) The function to call when the count is 0</param>
	public static void RepeatAscending (float time, float rate, int max, Action<int> onInvoke, Action onEnd=null) {
		int count = 0;
		InvokeWhileTrue (time, rate, () => { return count < max; }, () => {
			onInvoke (count);
			count ++;
			if (count == max && onEnd != null)
				onEnd ();
		});
	}

	public static void RepeatAscending (float rate, int max, Action<int> onInvoke, Action onEnd=null) {
		RepeatAscending (0f, rate, max, onInvoke, onEnd);
	}

	/// <summary>
	/// Makes a www request and sends a response callback
	/// </summary>
	/// <param name="address">The URL to request</param>
	/// <param name="onResponse">The callback when a response has been received</param>
	public static void WWW (string address, Action<WWW> onResponse) {
		CoMb.Instance.StartCoroutine (CoWWW (address, onResponse));
	}

	/// <summary>
	/// Makes a www request and sends a response callback if the response happens before timing out
	/// </summary>
	/// <param name="address">The URL to request</param>
	/// <param name="timeout">How long to wait before timing out</param>
	/// <param name="onResponse">The callback when a response has been received</param>
	/// <param name="onTimeout">The callback when the request times out</param>
	public static void WWW (string address, float timeout, Action<WWW> onResponse, Action<string> onTimeout) {
		CoMb.Instance.StartCoroutine (CoWWW (address, timeout, onResponse, onTimeout));
	}

	static IEnumerator CoWaitForSeconds (float seconds, Action onEnd) {
		float e = 0f;
		while (e < seconds) {
			e += Time.deltaTime;
			yield return null;
		}
		onEnd ();
	}

	static IEnumerator CoWaitForFixedUpdate (Action onEnd) {
		yield return new WaitForFixedUpdate ();
		onEnd ();
	}

	static IEnumerator CoCoroutine (float duration, Action<float> action, Action onEnd) {
		
		float e = 0f;

		while (e < duration && actions.Contains (action)) {
			e += Time.deltaTime;
			action (e / duration);
			yield return null;
		}

		actions.Remove (action);

		if (onEnd != null)
			onEnd ();
	}

	static IEnumerator CoYieldWhileTrue (Func<bool> condition, Action onEnd) {
		while (condition ()) yield return null;
		onEnd ();
	}

	static IEnumerator CoRunWhileTrue (Func<bool> condition, Action onRun, Action onEnd) {
		while (condition ()) {
			onRun ();
			yield return null;
		}
		if (onEnd != null)
			onEnd ();
	}

	static IEnumerator CoWWW (string address, Action<WWW> onResponse) {
		WWW www = new WWW (address);
		yield return www;
		onResponse (www);
	}

	static IEnumerator CoWWW (string address, float timeout, Action<WWW> onResponse, Action<string> onTimeout) {

		float e = 0f;
		WWW www = new WWW (address);

		while (e < timeout && !www.isDone) {
			e += Time.deltaTime;
			yield return www;
		}

		if (e >= timeout || www.error != null)
			onTimeout (www.error);
		else
			onResponse (www);
	}
}

public class CoMb : MonoBehaviour {

	static CoMb instance = null;
	static public CoMb Instance {
		get {
			if (instance == null) {
				instance = UnityEngine.Object.FindObjectOfType (typeof (CoMb)) as CoMb;
				if (instance == null) {
					GameObject go = new GameObject ("CoMb");
					go.hideFlags = HideFlags.HideInHierarchy;
					DontDestroyOnLoad (go);
					instance = go.AddComponent<CoMb> ();
				}
			}
			return instance;
		}
	}
}

public static class CoExtensionMethods {

	public static void MoveTo (this Transform transform, Vector3 target, float speed, Action onEnd=null) {

		Vector3 startPosition = transform.position;
		float distance = Vector3.Distance (startPosition, target);

		Co2.StartCoroutine (distance / speed, (float p) => {
			transform.position = Vector3.Lerp (startPosition, target, p);
			transform.LookAt (target);
		}, onEnd);
	}
}