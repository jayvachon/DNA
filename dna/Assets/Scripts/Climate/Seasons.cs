using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Seasons : MonoBehaviour, IWeatherSystem {

		public Pattern[] Patterns {
			get { return new Pattern[] { precipitation, temperature }; }
		}

		[SerializeField] NoisySignal precipitation;
		[SerializeField] NoisySignal temperature;

		void OnEnable () {
			precipitation = new NoisySignal (new Noise (60f, 0.5f), new Wave (240f, 1f));
			temperature = new NoisySignal (new Noise (60f, 0.5f), new Wave (240f, 1f));
		}

		public void Advance () {
			precipitation.Update ();
			temperature.Update ();
		}
	}
}