using UnityEngine;
using System.Collections;

public class Fertility {

	public const int TierCount = 6;
	public readonly float DistanceToCenter;
	public readonly float MaxValue;
	public readonly int Value;

	static Color[] colors;
	public static Color[] Colors {
		get {
			if (colors == null) {
				colors = new Color[] {
					new Color (0.604f, 0.906f, 0.914f),
					new Color (0.576f, 0.98f, 0f),
					new Color (0.98f, 0.604f, 0.784f),
					new Color (0.114f, 0.589f, 0.961f),
					new Color (0.251f, 0.843f, 0.192f),
					new Color (0.686f, 0.301f, 1f)
				};
			}
			return colors;
		}
	}

	public static float[] Multipliers {
		get { return new float[] { 1f, 1.2f, 1.5f, 2f, 3f, 5f }; }
	}

	public Fertility (float distanceToCenter, float maxValue) {
		DistanceToCenter = distanceToCenter;
		MaxValue = maxValue;
		float f = (DistanceToCenter * 2f) * MaxValue;
		Value = Mathf.Min (TierCount-1, Mathf.RoundToInt (f * (float)TierCount));
	}
}
