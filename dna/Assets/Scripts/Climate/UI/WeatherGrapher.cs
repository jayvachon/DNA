using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public class WeatherGrapher : MonoBehaviour {

		public WeatherSystems systems;
		List<PatternGrapher> graphs;

		void Start () {

			graphs = new List<PatternGrapher> ();

			foreach (var system in systems.Systems) {

				IWeatherSystem sys = system.Value;
				Dictionary<string, Pattern> sysPatterns = sys.Patterns;

				foreach (var kv in sysPatterns) {

					Pattern p = kv.Value;
					PatternGrapher g = ObjectPool.Instantiate<PatternGrapher> ();

					g.transform.SetParent (transform);
					g.transform.Reset ();
					g.SetPattern (p, sys.Name + ": " + p.Name);
					graphs.Add (g);
				}
			}

			float increment = 1f/(float)graphs.Count;
			for (int i = 0; i < graphs.Count; i ++) {
				graphs[i].SetScreenPosition (increment*i+increment*0.5f);
			}
		}
	}
}