using UnityEngine;
using System.Collections;
using GameInput;

public class RoadRender : MBRefs, IClickable, IHoverable {

	public virtual InputLayer[] IgnoreLayers {
		get { return new InputLayer[] { InputLayer.UI }; }
	}

	Road road = null;
	Road Road {
		get {
			if (road == null) {
				road = Parent.GetComponent<Road> ();
			}
			return road;
		}
	}

	public void OnHoverEnter () {
		Road.OnHoverEnter ();
	}

	public void OnHoverExit () {
		Road.OnHoverExit ();
	}

	public void OnClick (ClickSettings clickSettings) {
		if (clickSettings.left)
			Road.OnClick ();
	}

	public void OnHover () {}
}
