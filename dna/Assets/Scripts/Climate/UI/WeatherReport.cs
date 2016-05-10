using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class WeatherReport : MonoBehaviour {

		public WeatherSystems systems;

		public SystemReport precipitation;
		public SystemReport temperature;
		public SystemReport wind;
		public SystemReport sea;

		void Awake () {
			precipitation.patternName.text = "Precipitation";
			temperature.patternName.text = "Temperature";
			wind.patternName.text = "Wind";
			sea.patternName.text = "Sea";
		}

		void Update () {
			precipitation.value.text = Mathf.Round (systems.Precipitation * 100).ToString () + "%";
			temperature.value.text = Mathf.Round (systems.Temperature * 100).ToString () + "%";
			wind.value.text = Mathf.Round (systems.Wind * 100).ToString () + "%";
			sea.value.text = Mathf.Round (systems.Sea * 100).ToString () + "%";
		}
	}
}