using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class EfficiencyManager : MonoBehaviour {

		void Awake () {
			// UnitManager.AddListener<Laborer> (OnUpdateUnits);
			UnitManager.AddListener<Laborer> (OnUpdateLaborers);
			UnitManager.AddListener<Unit> (OnUpdateUnits);
		}

		void OnUpdateLaborers (UpdateUnitsEvent<Laborer> e) {
			Debug.Log ("LABORERS: " + e.Units.Count);
			// Debug.Log (UnitManager.GetUnitsOfType<Laborer> ().Count);
		}

		void OnUpdateUnits (UpdateUnitsEvent<Unit> e) {
			Debug.Log ("UNITS: " + e.Units.Count);
		}

		/*void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("HOUSE: " + UnitManager.GetAllUnitsOfType<House> ().Count);
				Debug.Log ("ILaborDependent: " + UnitManager.GetAllUnitsOfType<ILaborDependent> ().Count);
			}
		}*/
	}
}