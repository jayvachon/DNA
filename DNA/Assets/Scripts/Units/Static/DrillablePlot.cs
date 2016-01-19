using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA.Units {

	public class DrillablePlot : Plot {

		public override string Name { 
			get { return "DrillablePlot"; }
		}

		public override string Description {
			get { return "Build a milkshake derrick on this plot to drill for milkshakes. It's valued at " + (FertilityTier+1) + "/" + (Fertility.TierCount+1) + "."; }
		}

		protected override void OnSetFertility (int fertility) {
			unitRenderer.SetColors (Fertility.Colors[fertility]);
		}

		protected override void DestroyThis () {
			DestroyThis<DrillablePlot> ();
		}
	}
}