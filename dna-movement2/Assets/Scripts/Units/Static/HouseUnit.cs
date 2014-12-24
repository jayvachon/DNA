using UnityEngine;
using System.Collections;

public class HouseUnit : StaticUnit, IElderHoldable {

	ElderHolder elders = new ElderHolder ();
	public ElderHolder Elders {
		get { return elders; }
	}
}
