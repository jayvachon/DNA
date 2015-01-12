using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitColorHandler : System.Object {

	[SerializeField] Color defaultColor;
	[SerializeField] Color selectColor;
	
	public Color DefaultColor {
		get { return defaultColor; }
	}

	public Color SelectColor {
		get { return selectColor; }
	}

	Renderer renderer;

	public bool Selected {
		set {
			if (value) {
				renderer.SetColor (selectColor);
			} else {
				renderer.SetColor (defaultColor);
			}
		}
	}

	public void Init (Renderer renderer) {
		this.renderer = renderer;
		Selected = false;
	}
}