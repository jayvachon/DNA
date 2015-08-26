using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using GameInput;
using GameEvents;

namespace Units {

	public class DrillablePlot : Plot {

		public override string Description {
			get { return "This plot can be drilled for oil. " + positionInSpiral; }
		}

		float positionInSpiral = 0f;
		public float PositionInSpiral { 
			get { return positionInSpiral; }
			set {
				positionInSpiral = value;
				MilkshakeProduction mp = new MilkshakeProduction (positionInSpiral);
				//Debug.Log (mp.Production + ", $" + mp.Cost);
			}
		}

		protected override void Start () {
			base.Start ();
			PerformableActions.Add (
				new GenerateUnit<MilkshakePool, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Milkshake Derrick (15M)");
			SetActiveActions ();
		}
	}
}