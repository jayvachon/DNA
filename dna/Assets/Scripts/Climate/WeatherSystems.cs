using UnityEngine;
using System.Linq;
using System.Collections;

namespace DNA.Climate {

	public class WeatherSystems : MonoBehaviour {

		// IWeatherSystem[] systems;
		Dictionary<string, IWeatherSystem> systems;
		// public IWeatherSystem[] Systems {
		public Dictionary<string, IWeatherSystem> {
			get {
				IWeatherSystem sys = transform.GetChildren ()
					.FindAll (x => x.GetComponent<MonoBehaviour> () is IWeatherSystem)
					.ConvertAll (x => x.GetComponent<MonoBehaviour> () as IWeatherSystem)
					.ToArray ();
				return systems;
			}
		}

		void Update () {
			for (int i = 0; i < Systems.Length; i ++)
				Systems[i].Advance ();
		}
	}
}