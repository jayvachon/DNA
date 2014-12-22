using UnityEngine;
using System.Collections;

public class ActionManager : MonoBehaviour {

	public static ActionManager instance = null;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public void StartAction (IActionable point, IActionable visitor) {
		StartCoroutine (PerformAction (point, visitor));
	}

	IEnumerator PerformAction (IActionable point, IActionable visitor) {
		
		Action action = point.MyActionsList.ActiveAction;
		action.OnStartAction ();
		point.OnArrive ();
		visitor.OnArrive ();

		float time = action.time;
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			action.PerformAction (eTime / time, visitor);
			yield return null;
		}
		
		action.OnEndAction ();
		point.OnDepart ();
		visitor.OnDepart ();
	}
}
