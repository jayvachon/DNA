using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Storm : MonoBehaviour {

		// Signal signal;
		// Noise noise;

		public Grapher grapher;

		public NoisySignal precipitation;

		void OnEnable () {
			precipitation = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			grapher.SetPattern (precipitation);
			// wind = new NoisySignal (new Noise ())
			/*signal = new Signal () {
				Frequency = 60f,
				Amplitude = 1f
			};
			noise = new Noise () {
				Frequency = 60f,
				Amplitude = 1f
			};*/
		}

		void Update () {
			// signal.Update ();
			// noise.Update ();
			// Debug.Log (noise);
			// Debug.Log (signal);
		}
	}
}