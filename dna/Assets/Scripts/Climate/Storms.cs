using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Storms : MonoBehaviour {

		public NoisySignal precipitation;
		public NoisySignal wind;

		public PatternGrapher precipitationGrapher;
		public PatternGrapher windGrapher;

		void OnEnable () {
			precipitation = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			wind = new NoisySignal (new Noise (20f, 1f), new Wave (60f, 0.3f));
			precipitationGrapher.SetPattern (precipitation);
			windGrapher.SetPattern (wind);
		}
	}
}