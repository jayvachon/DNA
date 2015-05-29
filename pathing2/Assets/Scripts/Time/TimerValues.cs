using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerValues : MonoBehaviour {

	static TimerValues instance = null;
	static public TimerValues Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (TimerValues)) as TimerValues;
				if (instance == null) {
					GameObject go = new GameObject ("TimerValues");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<TimerValues>();
				}
			}
			return instance;
		}
	}

	public float Year = 5f;
	float pathSpeed = 10f;
	public float PathSpeed {
		get { return pathSpeed; }
		set { pathSpeed = value; }
	}

	Dictionary<string, float> actionTimes;
	Dictionary<string, float> ActionTimes {
		get {
			if (actionTimes == null) {
				actionTimes = new Dictionary<string, float> ();

				// GenerateUnits
				actionTimes.Add ("GenerateClinic", 0f);
				actionTimes.Add ("GenerateCoffeePlant", 0f);
				actionTimes.Add ("GenerateMilkshakePool", 0f);
				actionTimes.Add ("GenerateJacuzzi", 0f);
				actionTimes.Add ("GenerateLaborer", 0f);

				// GenerateItems
				actionTimes.Add ("GenerateYear", 1f);
				actionTimes.Add ("GenerateCoffee", 0.75f);
				actionTimes.Add ("GenerateHappiness", 1f);

				// ConsumeItems
				actionTimes.Add ("ConsumeHappiness", 0.75f);
				//actionTimes.Add ("ConsumeMilkshake", 5f);
				//actionTimes.Add ("ConsumeCoffee", 5f);
				actionTimes.Add ("ConsumeYear", 5f);

				// CollectItems
				actionTimes.Add ("CollectMilkshake", 0.5f);
				actionTimes.Add ("CollectCoffee", 0.5f);
				actionTimes.Add ("CollectHappiness", 0.1f);

				// DeliverItems
				actionTimes.Add ("DeliverMilkshake", 0.5f);
				actionTimes.Add ("DeliverCoffee", 0.5f);
				actionTimes.Add ("DeliverHappiness", 0.5f);
				actionTimes.Add ("DeliverYear", 0f);

			}
			return actionTimes;
		}
	}

	public float GetActionTime (string id) {
		float time;
		if (ActionTimes.TryGetValue (id, out time)) {
			return time * Year;
		}
		return -1f;
	}

	#if VARIABLE_TIME
	
	bool showOptions = false;
	bool showGenerate = false;
	bool showConsume = false;
	bool showCollect = false;
	bool showDeliver = false;
	Rect options = new Rect (0, 0, 300, 500);
	
	public void DrawOptions () {
		GUI.color = Color.black;
		showOptions = GUILayout.Toggle (showOptions, "Show duration settings");
		if (!showOptions) {
			return;
		}
		GUILayout.Label ("Duration settings");
		Year = DrawFloatSlider (Year, 1, 20, "Year (secs)");
		PathSpeed = DrawFloatSlider (PathSpeed, 1, 20, "Path Speed");
		GUILayout.Label ("Action times (% of year)");
		showGenerate = GUILayout.Toggle (showGenerate, "Generate");
		if (showGenerate) {
			DrawActionTimeSlider ("GenerateCoffee", "(Coffee plant) Coffee");
			DrawActionTimeSlider ("GenerateHappiness", "(Jacuzzi) Happiness");
		}
		showConsume = GUILayout.Toggle (showConsume, "Consume");
		if (showConsume) {
			DrawActionTimeSlider ("ConsumeHappiness", "(Laborer) Happiness");
			DrawActionTimeSlider ("ConsumeYear", "(Remains) Year");
		}
		showCollect = GUILayout.Toggle (showCollect, "(Laborer) Collect");
		if (showCollect) {
			DrawActionTimeSlider ("CollectCoffee", "Coffee");
			DrawActionTimeSlider ("CollectMilkshake", "Milkshake");
			DrawActionTimeSlider ("CollectHappiness", "Happiness");
		}
		showDeliver = GUILayout.Toggle (showDeliver, "(Laborer) Deliver");
		if (showDeliver) {
			DrawActionTimeSlider ("DeliverCoffee", "Coffee");
			DrawActionTimeSlider ("DeliverMilkshake", "Milkshake");
			DrawActionTimeSlider ("DeliverHappiness", "Happiness");
		}
	}

	void DrawActionTimeSlider (string key, string label) {
		actionTimes[key] = DrawFloatSlider (actionTimes[key], 0.1f, 5f, label, true);
	}

	float DrawFloatSlider (float value, float from, float to, string label, bool percentage=false) {
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label, new GUILayoutOption[] {GUILayout.Width (80)});
		float v = GUILayout.HorizontalSlider (value, from, to, new GUILayoutOption[] {GUILayout.Width (120)});
		float displayValue = (percentage) ? v.RoundToDecimal (2) * 100f : v.RoundToDecimal (2);
		string displayString = (percentage) ? displayValue.ToString () + "%" : displayValue.ToString ();
		GUILayout.Label (displayString, new GUILayoutOption[] {GUILayout.Width (40)});
		GUILayout.EndHorizontal ();
		return v;
	}
	#endif
}
