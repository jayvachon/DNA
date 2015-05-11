using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class OccupyBedEnabledState : EnabledState {

		public override bool Enabled {
			get { return !Occupying; }
		}

		public bool Occupying { get; set; }

		public OccupyBedEnabledState (bool occupying) {
			Occupying = occupying;
		}
	}
}