using UnityEngine;
using System.Collections;
using DNA.EventSystem;

namespace DNA.Units {

	public class EfficiencyManager : MonoBehaviour {

		void Awake () {
			UnitManager.AddListener<Laborer> (OnUpdateLaborers);
			UnitManager.onUpdate += OnUpdateUnits;
			Events.instance.AddListener<UpdateAccessibilityEvent> (OnUpdateAccessibility);
		}

		void OnUpdateLaborers (UpdateUnitsEvent<Laborer> e) {
			UpdateEfficiency ();
		}

		void OnUpdateUnits () {
			UpdateEfficiency ();
		}

		void OnUpdateAccessibility (UpdateAccessibilityEvent e) {
			UpdateEfficiency ();
		}

		void UpdateEfficiency () {
			int laborDependentCount = UnitManager.GetAllUnitsOfType<IWorkplace> ().FindAll (x => x.Accessible).Count;
			int laborerCount = UnitManager.GetUnitsOfType<Laborer> ().Count;
			// Debug.Log ("-----------------");
			// Debug.Log ("workplaces: " + laborDependentCount);
			// Debug.Log ("laborers: " + laborerCount);
			if (laborDependentCount > 0) 
				Debug.Log (((float)laborerCount / (float)laborDependentCount * 100) + "%");
		}
	}
}