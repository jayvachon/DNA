#undef FAST_FORWARD
using UnityEngine;
using System.Collections;

public class TimerValues {
	static readonly float workRetirementRatio = 0.541f;
	#if FAST_FORWARD
	public static float Retirement { get { return 15; } }
	#else
	public static float Retirement { get { return 180; } }
	#endif
	public static float Death { get { return Retirement - Retirement * workRetirementRatio; } }
}
