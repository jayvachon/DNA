﻿using UnityEngine;
using System.Collections;

public class HouseUnit : StaticUnit, IElderHoldable {

	ElderHolder elders = new ElderHolder ();
	public ElderHolder Elders {
		get { return elders; }
	}

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList = new HouseActionsList ();
		elders.Add (1);
	}
}
