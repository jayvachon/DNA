using UnityEngine;
using System.Collections;

public class HospitalUnit : StaticUnit {

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList = new HospitalActionsList ();
	}
}
