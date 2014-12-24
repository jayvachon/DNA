using UnityEngine;
using System.Collections;

public interface IActionable {

	ActionsList MyActionsList { get; }
	
	void ShowActions ();
	void HideActions ();
	void OnArrive ();
	void OnPerform (float progress);
	void OnDepart ();
}
