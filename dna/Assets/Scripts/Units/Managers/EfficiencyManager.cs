using UnityEngine;
using System.Collections;
using DNA.EventSystem;

namespace DNA.Units {

	public class EfficiencyManager : MonoBehaviour {

		static EfficiencyManager instance = null;
		static public EfficiencyManager Instance {
			get {
				if (instance == null) {
					instance = UnityEngine.Object.FindObjectOfType (typeof (EfficiencyManager)) as EfficiencyManager;
					if (instance == null) {
						GameObject go = new GameObject ("EfficiencyManager");
						instance = go.AddComponent<EfficiencyManager> ();
					}
				}
				return instance;
			}
		}

		public float Efficiency { get; private set; }

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
				Efficiency = (float)laborerCount * 0.33f / (float)laborDependentCount;
				// Debug.Log (((float)laborerCount / (float)laborDependentCount * 100) + "%");
		}
	}
}