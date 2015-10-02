using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Tasks;

public class DebugMenu : MonoBehaviour {

	void OnGUI () {
		if (GUILayout.Button ("Plan  road")) {
			if (PlayerActionState.State != ActionState.RoadConstruction)
				PlayerActionState.Set (ActionState.RoadConstruction);
			else if (PlayerActionState.State == ActionState.RoadConstruction)
				PlayerActionState.Set (ActionState.Idle);
		}
		if (GUILayout.Button ("Buy road")) {
			Player.Instance.PerformableTasks[typeof (BuildRoad)].Start ();
			PlayerActionState.Set (ActionState.Idle);
		}
	}
}
