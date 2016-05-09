using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Tides : MonoBehaviour, IWeatherSystem {

		public Pattern[] Patterns {
			get { return new Pattern[] { seaLevel }; }
		}

		[SerializeField] Wave seaLevel;

		public PatternGrapher tideGrapher;

		void OnEnable () {
			seaLevel = new Wave (30f);
			tideGrapher.SetPattern (seaLevel);
		}

		public void Advance () {
			seaLevel.Update ();
		}
	}
}