using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class EfficiencyManager : MonoBehaviour {

		void Awake () {
			UnitManager.AddListener<Laborer> (OnUpdateLaborers);
			UnitManager.onUpdate += OnUpdateUnits;
		}

		void OnUpdateLaborers (UpdateUnitsEvent<Laborer> e) {
			// Debug.Log ("LABORERS: " + e.Units.Count);
			// Debug.Log (UnitManager.GetUnitsOfType<Laborer> ().Count);
		}

		void OnUpdateUnits () {
			int laborDependentCount = UnitManager.GetAllUnitsOfType<ILaborDependent> ().Count;
			int laborerCount = UnitManager.GetUnitsOfType<Laborer> ().Count;
			Debug.Log ("-----------------");
			Debug.Log (laborDependentCount);
			Debug.Log (laborerCount);
			if (laborDependentCount > 0) 
				Debug.Log ((float)laborerCount / (float)laborDependentCount);
		}

		/*void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("HOUSE: " + UnitManager.GetAllUnitsOfType<House> ().Count);
				Debug.Log ("ILaborDependent: " + UnitManager.GetAllUnitsOfType<ILaborDependent> ().Count);
			}
		}*/
	}
}