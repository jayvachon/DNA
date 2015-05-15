using UnityEngine;
using System.Collections;
using GameInventory;
using Units;

namespace GameActions {

	public class DeliverAllYears : DeliverAllItems<YearHolder> {

		public override EnabledState EnabledState {
			get { return new DefaultEnabledState (); }
		}
	}
}