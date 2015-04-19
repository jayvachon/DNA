#define FAST_FORWARD
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerValues {
	static readonly float workRetirementRatio = 0.541f;
	#if FAST_FORWARD
	public static float Retirement { get { return 5; } }
	#else
	public static float Retirement { get { return 180; } }
	#endif
	public static float Death { get { return Retirement - Retirement * workRetirementRatio; } }

	// TODO: have units use these times instead of hardcoded values
	static Dictionary<string, float> actionTimes;
	public static Dictionary<string, float> ActionTimes {
		get {
			if (actionTimes == null) {
				actionTimes = new Dictionary<string, float> ();

				actionTimes.Add ("CollectIceCream", 0.5f);
				actionTimes.Add ("DeliverIceCream", 0.5f);
				actionTimes.Add ("CollectMilk", 0.5f);
				actionTimes.Add ("DeliverMilk", 0.5f);
				actionTimes.Add ("CollectMilkshake", 0.5f);
				actionTimes.Add ("DeliverMilkshake", 0.5f);
				actionTimes.Add ("CollectElder", 1f);
				actionTimes.Add ("DeliverElder", 1f);
				actionTimes.Add ("CollectHappiness", 0.1f);
				actionTimes.Add ("DeliverHappiness", 0.1f);

				actionTimes.Add ("ConsumeMilkshake", 1);
				actionTimes.Add ("HealElder", 5);
			}
			return actionTimes;
		}
	}
}
