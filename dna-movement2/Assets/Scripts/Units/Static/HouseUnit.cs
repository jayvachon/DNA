using UnityEngine;
using System.Collections;

public class HouseUnit : StaticUnit {

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList = new HouseActionsList ();
	}
}
