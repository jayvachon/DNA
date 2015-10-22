using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

public static class EmissionsManager {

	static float emissionsRate = 0f;
	public static float EmissionsRate {
		get { return emissionsRate; }
		set { 
			emissionsRate = value; 
			if (Sea.Instance != null)
				Sea.Instance.Rate = emissionsRate;
		}
	}

	// Units' emissions. Values are relative & should be set between 0 and 1
	static Dictionary<string, float> emissionsValues = new Dictionary<string, float> () {
		{ "Milkshake Derrick", 0.25f },
		{ "Jacuzzi", 0.15f },
		{ "Clinic", 0.1f },
		{ "Laborer", 0.05f }
	};

	public static void AddUnit (Unit unit) {

		// Give the sea a frame to find itself
		Coroutine.WaitForSeconds (1f, () => {
			float emissionValue;
			if (emissionsValues.TryGetValue (unit.Name, out emissionValue)) {
				EmissionsRate += emissionValue;
			}
		});
	}

	public static void RemoveUnit (Unit unit) {
		float emissionValue;
		if (emissionsValues.TryGetValue (unit.Name, out emissionValue)) {
			EmissionsRate -= emissionValue;
		}
	}

	public static void Reset () {
		EmissionsRate = 0f;
	}
}
