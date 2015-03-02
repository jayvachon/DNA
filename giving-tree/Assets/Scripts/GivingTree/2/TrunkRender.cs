using UnityEngine;
using System.Collections;

public class TrunkRender : MBRefs {

	protected override void Awake () {
		base.Awake ();
		renderer.SetColor (Color.yellow);
	}
}
