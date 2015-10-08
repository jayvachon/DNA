using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Tasks;
using DNA.Units;

namespace DNA {

	public class DebugMenu : MonoBehaviour {

		void OnGUI () {
			GUILayout.Space (40);
			if (GUILayout.Button ("Plan road")) {
				Player.Instance.SetConstructionPen<BuildRoad> ();
			}
			if (GUILayout.Button ("Birth Coffee Plant")) {
				Player.Instance.SetConstructionPen<GenerateUnit<CoffeePlant>> ();
			}
			if (GUILayout.Button ("Birth Milkshake Derrick")) {
				Player.Instance.SetConstructionPen<GenerateUnit<MilkshakePool>> ();
			}
		}
	}
}