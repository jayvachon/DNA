using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

public static class EmissionsManager {

	static float emissionsRate = 0f;

	public delegate void OnUpdate (float val);

	public static OnUpdate onUpdate;

	public static void AddEmissions (float val) {
		if (!Mathf.Approximately (val, 0f)) {
			emissionsRate += val;
			SendUpdateMessage ();
		}
	}

	public static void RemoveEmissions (float val) {
		if (!Mathf.Approximately (val, 0f)) {
			emissionsRate -= val;
			SendUpdateMessage ();
		}
	}

	static void SendUpdateMessage () {
		if (onUpdate != null)
			onUpdate (emissionsRate);
	}

	public static void Reset () {
		emissionsRate = 0f;
	}
}
