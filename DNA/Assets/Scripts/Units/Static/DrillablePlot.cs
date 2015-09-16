using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA.Units {

	public class DrillablePlot : Plot {

		public int Index { get; set; }

		public override string Description {
			get { 
				return "This plot can be drilled for milkshakes. \n" 
					+ mp.Tier.Return + " return\n"
					+ "index: " + Index;
			}
		}

		MilkshakeProduction mp;
		float positionInSpiral = 0f;
		public float PositionInSpiral { 
			get { return positionInSpiral; }
			set {
				positionInSpiral = value;
				mp = new MilkshakeProduction (positionInSpiral);
			}
		}

		protected override void Start () {
			base.Start ();
			unitRenderer.colorHandler.DefaultColor = mp.Tier.Color;
			unitRenderer.colorHandler.SelectColor = Color.red;
		}

		protected override void DestroyThis () {
			DestroyThis<DrillablePlot> ();
		}
	}
}