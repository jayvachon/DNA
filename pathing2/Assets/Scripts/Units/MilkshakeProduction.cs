using UnityEngine;
using System.Collections;

public class MilkshakeProduction {

	// Minimum, Maximum milkshakes that a derrick will produce
	int[] production = new int[2] { 15, 500 };

	// Minimum, Maximum cost of building a derrick
	int[] cost = new int[2] { 5, 100 };

	// Minimum, Maximum +/- random deviation from position
	float[] randomness = new float[2] { 0.1f, 0.5f };

	EPPZEasing easing = EPPZEasing.EasingForType (EPPZEasing.EasingType.Ease_In_Circular);

	public int Production { get; private set; }
	public int Cost { get; private set; }
	public ProductionTier Tier { get; private set; }

	public MilkshakeProduction (float position) {
		float r = ValueBetweenMinAndMax (randomness[0], randomness[1], position);
		float p = Mathf.Clamp01 (Random.Range (-r, r) + position);

		int index = ValueBetweenMinAndMax (0, 4, p);
		Tier = new ProductionTier (index);

		// Not using these
		Production = ValueBetweenMinAndMax (production[0], production[1], p);
		Cost = ValueBetweenMinAndMax (cost[0], cost[1], p);
	}

	float Curve (float position) {
		return easing.ValueForInput (position);
	}

	int ValueBetweenMinAndMax (int min, int max, float position) {
		return Mathf.RoundToInt ((float)min + (((float)max - (float)min) * position));
	}

	float ValueBetweenMinAndMax (float min, float max, float position) {
		return min + ((max - min) * position);
	}
}
