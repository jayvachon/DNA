﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public class WeatherGrapher : MonoBehaviour {

		public WeatherSystems systems;
		List<PatternGrapher> graphs;

		void Start () {

			graphs = new List<PatternGrapher> ();

			for (int i = 0; i < systems.Systems.Length; i ++) {

				IWeatherSystem system = systems.Systems[i];
				Pattern[] sysPatterns = system.Patterns;

				for (int j = 0; j < sysPatterns.Length; j ++) {

					Pattern p = sysPatterns[j];
					PatternGrapher g = ObjectPool.Instantiate<PatternGrapher> ();

					g.transform.SetParent (transform);
					g.transform.Reset ();
					g.SetPattern (p, system.Name + ": " + p.Name);
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