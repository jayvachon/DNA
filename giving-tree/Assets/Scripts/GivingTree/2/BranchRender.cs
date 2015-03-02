using UnityEngine;
using System.Collections;

public class BranchRender : MBRefs {

	protected override void Awake () {
		base.Awake ();
		renderer.SetColor (Color.red);
	}
}
