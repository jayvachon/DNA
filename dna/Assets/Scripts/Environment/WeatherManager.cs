using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour {

	public Rain rain;

	float stormThreshold = 0.67f;

	RainPattern rainPattern = new RainPattern ();
	WindPattern windPattern = new WindPattern ();

	void Update () {
		
		rainPattern.Update ();
		windPattern.Update ();

		rain.UpdateRain (rainPattern.Intensity);
		rain.UpdateWind (windPattern.Intensity, windPattern.Direction);
	}

	// Method 1

	class RainPattern {

		WeatherPattern intensityPattern = new WeatherPattern (0.001f);

		public float Intensity { get; private set; }

		public void Update () {
			Intensity = intensityPattern.Update ();
		}
	}

	class WindPattern {

		WeatherPattern intensityPattern = new WeatherPattern (0.01f);
		WeatherPattern directionPattern = new WeatherPattern (0.001f);

		public float Intensity { get; private set; }
		public float Direction { get; private set; }

		public void Update () {
			Intensity = intensityPattern.Update ();
			Direction = directionPattern.Update ();
		}
	}

	class WeatherPattern {

		public float Value { get; private set; }

		int idx = 0;
		float changeRate;
		float threshold;

		public WeatherPattern (float changeRate, float threshold=0.1f) {
			this.changeRate = changeRate;
			this.threshold = threshold;
			// idx = Random.Range (0, 10000);
		}

		public float Update () {
			Value = Mathf.Pow (Mathf.PerlinNoise ((++idx)*changeRate, 0), 4);
			Value = Value >= threshold ? Value : 0f;
			return Value;
		}
	}
}
