using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CostValues : MonoBehaviour {

	static CostValues instance = null;
	static public CostValues Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (CostValues)) as CostValues;
				if (instance == null) {
					GameObject go = new GameObject ("CostValues");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<CostValues>();
				}
			}
			return instance;
		}
	}

	Dictionary<string, int> costs;
	public Dictionary<string, int> Costs {
		get {
			if (costs == null) {
				costs = new Dictionary<string, int> ();
				costs.Add ("GenerateDistributor", 15);
				costs.Add ("GenerateCoffeePlant", 5);
				costs.Add ("GenerateJacuzzi", 20);
				costs.Add ("GenerateClinic", 25);
			}
			return costs;
		}
	}

	public int GetCost (string id) {
		int cost;
		if (Costs.TryGetValue (id, out cost)) {
			return cost;
		}
		return -1;
	}

	#if VARIABLE_COST

	bool showOptions = false;

	public void DrawOptions () {
		GUI.color = Color.black;
		showOptions = GUILayout.Toggle (showOptions, "Show cost settings");
		if (!showOptions) {
			return;
		}
		DrawCostValueSlider ("GenerateDistributor", "Laborer");
		DrawCostValueSlider ("GenerateCoffeePlant", "Coffee Plant");
		DrawCostValueSlider ("GenerateJacuzzi", "Jacuzzi");
		DrawCostValueSlider ("GenerateClinic", "Clinic");
	}

	void DrawCostValueSlider (string key, string label) {
		costs[key] = DrawIntSlider (costs[key], 1, 50, label);
	}

	int DrawIntSlider (int value, int from, int to, string label, bool percentage=false) {
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label, new GUILayoutOption[] {GUILayout.Width (80)});
		int v = (int)GUILayout.HorizontalSlider (value, from, to, new GUILayoutOption[] {GUILayout.Width (120)});
		int displayValue = (percentage) ? v * 100 : v;
		string displayString = (percentage) ? displayValue.ToString () + "%" : displayValue.ToString ();
		GUILayout.Label (displayString, new GUILayoutOption[] {GUILayout.Width (40)});
		GUILayout.EndHorizontal ();
		return v;
	}
	#endif
}
