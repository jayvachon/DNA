using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public interface IWeatherSystem {

		string Name { get; }
		Pattern[] Patterns { get; }
		void Advance ();
	}
}