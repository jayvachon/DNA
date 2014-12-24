using UnityEngine;
using System.Collections;

public class Cow : StaticUnit {
	
	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList.Add (new GenerateIceCreamAction ());
	}
}
