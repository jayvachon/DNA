using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA.Units {

	public class DrillablePlot : Plot {

		public override string Description {
			get { 
				return "An undeveloped plot valued at " + FertilityTier + "/" + Fertility.TierCount;
				/*return "This plot can be drilled for milkshakes. \n" 
					+ mp.Tier.Return + " return\n"
					+ "index: " + Index;*/
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

		protected override void OnSetFertility (int fertility) {
			unitRenderer.SetColors (Fertility.Colors[fertility]);
		}

		protected override void DestroyThis () {
			DestroyThis<DrillablePlot> ();
		}
	}
}