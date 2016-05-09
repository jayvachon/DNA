using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class WeatherSystem : MonoBehaviour {

		public Storms storms;
		// public PatternGrapher precipitationGrapher;

		void OnEnable () {
			// precipitationGrapher.SetPattern (storms.precipitation);
		}
	}
}