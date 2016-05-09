using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Storms : MonoBehaviour, IWeatherSystem {

		public Pattern[] Patterns {
			get { return new Pattern[] { precipitation, wind, temperature }; }
		}

		[SerializeField] NoisySignal precipitation;
		[SerializeField] NoisySignal wind;
		[SerializeField] NoisySignal temperature;

		public PatternGrapher precipitationGrapher;
		public PatternGrapher windGrapher;
		public PatternGrapher temperatureGrapher;

		void OnEnable () {
			
			precipitation = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			wind = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			temperature = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			
			precipitationGrapher.SetPattern (precipitation);
			windGrapher.SetPattern (wind);
			temperatureGrapher.SetPattern (temperature);
		}

		public void Advance () {
			precipitation.Update ();
			wind.Update ();
			temperature.Update ();
		}
	}
}