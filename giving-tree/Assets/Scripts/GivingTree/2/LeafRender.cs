using UnityEngine;
using System.Collections;

public class LeafRender : MBRefs {

	protected override void Awake () {
		base.Awake ();
		GetComponent<Renderer>().SetColor (Color.green);
	}
}
