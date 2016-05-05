using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour {

	float stormThreshold = 0.67f;
	int eventIndex = 0;

	void Update () {
		Debug.Log (GetNextEvent ());
	}

	float GetNextEvent () {
		return Mathf.PerlinNoise ((++eventIndex)*0.001f, 0);
	}
}
