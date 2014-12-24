using UnityEngine;
using System.Collections;

public class HospitalUnit : StaticUnit, IElderHoldable {

	ElderHolder elders = new ElderHolder ();
	public ElderHolder Elders {
		get { return elders; }
	}
}
