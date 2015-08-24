using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using GameInput;
using GameEvents;

namespace Units {
	
	public class FertilePlot : Plot {

		public override string Description {
			get { return "Construct buildings or plants on fertile plots."; }
		}

		protected override void Start () {
			base.Start ();
			PerformableActions.Add (new GenerateUnit<CoffeePlant, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Coffee Plant (5M)");
			SetActiveActions ();
		}
	}
}
