using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public interface IWeatherSystem {

		Pattern[] Patterns { get; }
		void Advance ();
	}
}