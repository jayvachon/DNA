using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Renderer))]
public class UnitRender : MBRefs {

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	public void Init () {
		colorHandler.Init (renderer);
	}

	public void OnSelect () {
		colorHandler.Selected = true;
	}

	public void OnUnselect () {
		colorHandler.Selected = false;
	}
}
