﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	public class UnitRenderer : ObjectColor {

		// TODO: Move this to the UnitSettings model		
		static Dictionary<string, string> renderers;
		static Dictionary<string, string> Renderers {
			get {
				if (renderers == null) {
					renderers = new Dictionary<string, string> () {
						{ "clinic", "ClinicRenderer" },
						{ "collector", "CollectionCenterRenderer" },
						{ "coffee", "CoffeePlantRenderer" },
						{ "derrick", "DerrickRenderer" },
						{ "flower", "FlowerRenderer" },
						{ "tree", "GivingTreeRenderer" },
						{ "university", "UniversityRenderer" },
						{ "road", "RoadRenderer" },
						{ "plot", "PlotRenderer" },
						{ "drillable", "DrillablePlotRenderer" },
						{ "house", "HouseRenderer" },
						{ "construction", "ConstructionSiteRenderer" },
						{ "laborer", "LaborerRenderer" },
						{ "seed", "SeedRenderer" },
						{ "apartment", "ApartmentRenderer" }
					};
				}
				return renderers;
			}
		}

		public static string GetRenderer (string key) {
			string r;
			if (Renderers.TryGetValue (key, out r)) {
				return r;
			} else {
				throw new System.Exception ("No UnitRenderer exists for the key " + key);
			}
		}

		public virtual Vector3 Offset { get { return Vector3.zero; } }

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