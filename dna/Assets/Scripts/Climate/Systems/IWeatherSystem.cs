using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Climate {

	public interface IWeatherSystem {

		string Name { get; }
		Dictionary<string, Pattern> Patterns { get; }
		void Advance ();
	}
}