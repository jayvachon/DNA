using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Tides : MonoBehaviour, IWeatherSystem {

		public string Name {
			get { return "Tides"; }
		}

		public Pattern[] Patterns {
			get { return new Pattern[] { seaLevel }; }
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