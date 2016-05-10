using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public class Tides : MonoBehaviour, IWeatherSystem {

		public string Name {
			get { return "Tide"; }
		}

		Dictionary<string, Pattern> patterns;
		public Dictionary<string, Pattern> Patterns {
			get {
				if (patterns == null) {
					patterns = new Dictionary<string, Pattern> () {
						{ "sea", seaLevel }
					};
				}
				return patterns;
			}
		}

		[SerializeField] Wave seaLevel;

		void OnEnable () {
			seaLevel = new Wave (30f);
			seaLevel.Name = "Sea level";
		}

		public void Advance () {
			seaLevel.Update ();
		}
	}
}