using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public interface IGenerator {

		float Frequency { get; set; }
		float Amplitude { get; set; }
	}
}