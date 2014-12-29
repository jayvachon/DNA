using UnityEngine;
using System.Collections;

public class HospitalUnit : StaticUnit, IElderHoldable {

	ElderHolder elders = new ElderHolder ();
	public ElderHolder Elders {
		get { return elders; }
	}

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList = new HospitalActionsList ();
	}
}
