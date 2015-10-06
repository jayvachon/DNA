using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Tasks;

public class DebugMenu : MonoBehaviour {

	void OnGUI () {
		GUILayout.Space (40);
		if (GUILayout.Button ("Plan  road")) {
			if (PlayerActionState.State != ActionState.RoadConstruction)
				PlayerActionState.Set (ActionState.RoadConstruction);
			else if (PlayerActionState.State == ActionState.RoadConstruction)
				PlayerActionState.Set (ActionState.Idle);
		}
	}
}
