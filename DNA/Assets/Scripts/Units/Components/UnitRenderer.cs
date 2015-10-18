using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	public class UnitRenderer : ObjectColor {

		public void SetColors (Color def, Color? selected=null, Color? abandoned=null) {
			
			Color s = (selected == null)
				? Color.red
				: (Color)selected;

			Color a = (abandoned == null) 
				? HSBColor.Lerp (HSBColor.FromColor (def), HSBColor.Black, 0.67f).ToColor ()
				: (Color)abandoned;

			AddColor ("default", def);
			AddColor ("selected", s);
			AddColor ("abandoned", a);
			SetColor ("default");
		}

		public void SetAbandoned () {
			PrimaryColor = "abandoned";
			if (CurrentColor != "selected")
				SetColor ("abandoned");
		}

		public void OnSelect () {
			SetColor ("selected");
		}

		public void OnUnselect () {
			SetColor ();
		}
	}
}