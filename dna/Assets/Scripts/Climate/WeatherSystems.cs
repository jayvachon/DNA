using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public class WeatherSystems : MonoBehaviour {

		Dictionary<string, IWeatherSystem> systems;
		public Dictionary<string, IWeatherSystem> Systems {
			get {
				if (systems == null) {

					List<Transform> children = transform.GetChildren ();
					systems = new Dictionary<string, IWeatherSystem> ();

					foreach (Transform c in children) {
						IWeatherSystem ws = c.GetComponent<MonoBehaviour> () as IWeatherSystem;
						if (ws != null)
							systems.Add (ws.Name.ToLower (), ws);
					}
				}
				return systems;
			}
		}

		public float Precipitation {
			get { 
				return Pattern.Normalize (
					Pattern.Add (
						Systems["weather"].Patterns["precipitation"], 
						Systems["season"].Patterns["precipitation"]));
			}
		}

		public float Temperature {
			get {
				return Pattern.Normalize (
					Pattern.Add (
						Systems["weather"].Patterns["temperature"],
						Systems["season"].Patterns["temperature"]));
			}
		}

		public float Sea {
			get { 
				return Pattern.Normalize (
					Pattern.Add (
						Systems["tide"].Patterns["sea"],
						Systems["weather"].Patterns["sea"]));
			}
		}

		public float Wind {
			get { return Pattern.Normalize (Systems["weather"].Patterns["wind"].ValueAtCursor); }
		}

		void Update () {
			foreach (var kv in Systems)
				kv.Value.Advance ();
		}
	}
}