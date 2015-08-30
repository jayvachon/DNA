using UnityEngine;
using System.Collections;

namespace Units {

	[System.Serializable]
	public class UnitColorHandler : System.Object {

		[SerializeField] Color defaultColor;
		[SerializeField] Color selectColor;
		
		public Color DefaultColor {
			get { return defaultColor; }
			set { 
				defaultColor = value; 
				Selected = false;
			}
		}

		public Color SelectColor {
			get { return selectColor; }
			set { selectColor = Color.red; }
		}

		Renderer renderer;
		public Renderer Renderer {
			get { return renderer; }
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

		public void Init (Renderer renderer) {
			this.renderer = renderer;
			Selected = false;
		}
	}
}