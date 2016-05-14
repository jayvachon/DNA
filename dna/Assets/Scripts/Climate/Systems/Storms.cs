using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public class Storms : MonoBehaviour, IWeatherSystem {

		public string Name {
			get { return "Weather"; }
		}

		Dictionary<string, Pattern> patterns;
		public Dictionary<string, Pattern> Patterns {
			get {
				if (patterns == null) {
					patterns = new Dictionary<string, Pattern> () {
						{ "precipitation", precipitation },
						{ "wind", wind },
						{ "temperature", temperature },
						{ "sea", sea }
					};
				}
				return patterns;
			}
		}

		[SerializeField] NoisySignal precipitation;
		[SerializeField] NoisySignal wind;
		[SerializeField] NoisySignal temperature;
		[SerializeField] DelayedPattern sea;

		void OnEnable () {
			
			precipitation = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			wind = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			temperature = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			sea = new DelayedPattern (precipitation, 30f);
			
			precipitation.Name = "Precipitation";
			wind.Name = "Wind";
			temperature.Name = "Temperature";
			sea.Name = "Sea";
		}

		public void Advance () {
			precipitation.Update ();
			wind.Update ();
			temperature.Update ();
			sea.Update ();
		}
	}
}