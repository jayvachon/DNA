#define FAST_FORWARD
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerValues {

	#if FAST_FORWARD
	public static float year = 1f; // # of seconds in 1 year
	#else
	public static float year = 5f;
	#endif

	static readonly float workRetirementRatio = 0.541f;
	public static float Retirement { get { return 65f * year; } }
	public static float Death { get { return Retirement - Retirement * workRetirementRatio; } }

	static Dictionary<string, float> actionTimes;
	public static Dictionary<string, float> ActionTimes {
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
				actionTimes.Add ("GenerateCoffee", 1.25f);
				actionTimes.Add ("GenerateHappiness", 1f);

				// ConsumeItems
				actionTimes.Add ("ConsumeHappiness", 0.75f);
				actionTimes.Add ("ConsumeMilkshake", 5f);
				actionTimes.Add ("ConsumeCoffee", 5f);
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

				// Miscellaneous
				actionTimes.Add ("HealElder", 5f);
				actionTimes.Add ("OccupyBed", 0f);
			}
			return actionTimes;
		}
	}

	public static float GetActionTime (string id) {
		float time;
		if (ActionTimes.TryGetValue (id, out time)) {
			return time * year;
		}
		return -1f;
	}
}
