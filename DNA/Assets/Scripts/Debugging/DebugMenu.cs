using UnityEngine;
using System.Collections;
using DNA.Paths;

public class DebugMenu : MonoBehaviour {

	void OnGUI () {
		if (GUILayout.Button ("Plan  road")) {
			PlayerActionState.Set (ActionState.RoadConstruction);
		}
		if (GUILayout.Button ("Buy road")) {
			RoadConstructor.Instance.Build ();
			PlayerActionState.Set (ActionState.Idle);
		}
	}
}
