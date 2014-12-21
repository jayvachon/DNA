using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitColorHandler : System.Object {

	public Color defaultColor = Color.white;
	public Color selectColor = Color.red;
	Renderer renderer;

	public Color DefaultColor {
		get { return defaultColor; }
	}

	public Color SelectColor {
		get { return selectColor; }
	}

	public bool Selected {
		set {
			if (value) {
				renderer.SetColor (selectColor);
			} else {
				renderer.SetColor (defaultColor);
			}
		}
	}

	public UnitColorHandler (Renderer render) {
		this.renderer = renderer;
		Selected = false;
	}
}