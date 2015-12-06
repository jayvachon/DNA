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
					new Color (0.816f, 0.98f, 0.886f),
					new Color (0.98f, 0.706f, 0.792f),
					new Color (0.435f, 0.961f, 0.565f),
					new Color (0.918f, 0.243f, 0.388f),
					new Color (0f, 0.796f, 0.169f)
				};
			}
			return colors;
		}
	}

	public static float[] Multipliers {
		get { return new float[] { 1f, 1.1f, 1.2f, 1.33f, 1.5f, 1.75f }; }
	}

	public Fertility (float distanceToCenter, float maxValue) {
		DistanceToCenter = distanceToCenter;
		MaxValue = maxValue;
		float f = (DistanceToCenter * 1.5f) * MaxValue;
		Value = Mathf.Min (TierCount-1, Mathf.RoundToInt (f * (float)TierCount));
	}
}
