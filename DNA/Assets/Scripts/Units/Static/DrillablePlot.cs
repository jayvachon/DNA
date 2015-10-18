using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA.Units {

	public class DrillablePlot : Plot {

		public int Index { get; set; }

		public float DistanceToCenter {
			set {
				/*unitRenderer.SetColors (HSBColor.Lerp (
					HSBColor.FromColor (unitRenderer.GetColor (unitRenderer.PrimaryColor)), 
					HSBColor.Black, value)
						.ToColor ());*/
			}
		}

		public float Fertility {
			set {
				unitRenderer.SetColors (HSBColor.Lerp (
					HSBColor.FromColor (unitRenderer.GetColor (unitRenderer.PrimaryColor)), 
					HSBColor.Black, value)
						.ToColor ());
				Debug.Log (value);
			}
		}

		public override string Description {
			get { 
				return "terp";
				/*return "This plot can be drilled for milkshakes. \n" 
					+ mp.Tier.Return + " return\n"
					+ "index: " + Index;*/
			}
		}

		//MilkshakeProduction mp;
		float positionInSpiral = 0f;
		public float PositionInSpiral { 
			get { return positionInSpiral; }
			set {
				positionInSpiral = value;
				//mp = new MilkshakeProduction (positionInSpiral);
			}
		}

		protected override void OnEnable () {
			base.OnEnable ();
			SelectSettings.CanSelect = false;
		}

		protected override void Start () {
			base.Start ();

			// TODO Do this (tier business) based on y position

			//unitRenderer.colorHandler.DefaultColor = mp.Tier.Color;
			//unitRenderer.colorHandler.SelectColor = Color.red;
		}

		protected override void DestroyThis () {
			DestroyThis<DrillablePlot> ();
		}
	}
}