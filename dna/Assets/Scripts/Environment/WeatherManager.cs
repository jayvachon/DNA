using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour {

	public Rain rain;
	public FogController fog;

	// float stormThreshold = 0.67f;

	RainPattern rainPattern = new RainPattern ();
	WindPattern windPattern = new WindPattern ();

	// StormCurve small = new StormCurve (60f);
	StormCurve[] curves;

	void Awake () {
		curves = new StormCurve[3];
		for (int i = 0; i < curves.Length; i ++) {
			curves[i] = new StormCurve (60 * Mathf.Pow (i+1, 2));
		}
	}

	void Update () {
		
		rainPattern.Update ();
		windPattern.Update ();

		// rain.UpdateRain (rainPattern.Intensity);
		// rain.UpdateWind (windPattern.Intensity, windPattern.Direction);

		float val = 0f;
		for (int i = 0; i < curves.Length; i ++) {
			curves[i].Update ();
			val += curves[i].Value;
		}

		float intensity = Mathf.Pow (val/(float)curves.Length, 6);
		rain.UpdateRain (intensity);
		rain.UpdateWind (intensity, windPattern.Direction);
		fog.UpdateRain (intensity);
	}

	class StormCurve {

		public float Value { get; private set; }

		float rate;
		float time = 0f;

		public StormCurve (float rate) {
			this.rate = rate;
		}

		public void Update () {
			time += 1/rate*Time.deltaTime;
			Value = Mathf.Sin (Mathf.PI * 2 * time);
		}
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
