using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA.Units {

	public class DrillablePlot : Plot {

		public override string Description {
			get { return "An undeveloped plot valued at " + FertilityTier + "/" + Fertility.TierCount; }
		}

		protected override void OnEnable () {
			base.OnEnable ();
			PlayerActionState.onChange += OnChangePlayerActionState;
		}

		protected override void OnDisable () {
			base.OnDisable ();
			PlayerActionState.onChange -= OnChangePlayerActionState;
		}

		protected override void OnSetFertility (int fertility) {
			unitRenderer.SetColors (Fertility.Colors[fertility]);
		}

		protected override void DestroyThis () {
			DestroyThis<DrillablePlot> ();
		}

		void OnChangePlayerActionState (ActionState state) {
			if (state == ActionState.Construction) {
				SelectSettings.CanSelect = false;
			} else {
				SelectSettings.CanSelect = true;
			}
		}
	}
}