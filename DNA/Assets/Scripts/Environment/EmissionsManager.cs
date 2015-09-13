using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

public class EmissionsManager : MonoBehaviour {

	static EmissionsManager instance = null;
	static public EmissionsManager Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (EmissionsManager)) as EmissionsManager;
				if (instance == null) {
					GameObject go = new GameObject ("EmissionsManager");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<EmissionsManager>();
				}
			}
			return instance;
		}
	}

	float emissionsRate = 0f;
	public float EmissionsRate {
		get { return emissionsRate; }
		set { 
			emissionsRate = value; 
			Sea.Instance.Rate = emissionsRate;
		}
	}

	// Units' emissions. Values are relative & should be set between 0 and 1
	Dictionary<string, float> emissionsValues = new Dictionary<string, float> () {
		{ "Milkshake Derrick", 0.25f },
		{ "Jacuzzi", 0.15f },
		{ "Clinic", 0.1f },
		{ "Laborer", 0.05f }
	};

	public void AddUnit (Unit unit) {
		float emissionValue;
		if (emissionsValues.TryGetValue (unit.Name, out emissionValue)) {
			EmissionsRate += emissionValue;
		}
	}

	public void RemoveUnit (Unit unit) {
		float emissionValue;
		if (emissionsValues.TryGetValue (unit.Name, out emissionValue)) {
			EmissionsRate -= emissionValue;
		}
	}

	public void Reset () {
		EmissionsRate = 0f;
	}
}
