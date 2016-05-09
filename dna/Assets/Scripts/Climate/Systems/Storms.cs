using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Storms : MonoBehaviour, IWeatherSystem {

		public string Name {
			get { return "Weather"; }
		}

		public Pattern[] Patterns {
			get { return new Pattern[] { precipitation, wind, temperature }; }
		}

		[SerializeField] NoisySignal precipitation;
		[SerializeField] NoisySignal wind;
		[SerializeField] NoisySignal temperature;

		void OnEnable () {
			
			precipitation = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			wind = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			temperature = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			
			precipitation.Name = "Precipitation";
			wind.Name = "Wind";
			temperature.Name = "Temperature";
		}

		public void Advance () {
			precipitation.Update ();
			wind.Update ();
			temperature.Update ();
		}
	}
}