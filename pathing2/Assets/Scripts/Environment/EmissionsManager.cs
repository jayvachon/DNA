using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Units;

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
		{ "Milkshake Derrick", 1.0f },
		{ "Jacuzzi", 0.5f },
		{ "Clinic", 0.33f },
		{ "Laborer", 0.25f }
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
