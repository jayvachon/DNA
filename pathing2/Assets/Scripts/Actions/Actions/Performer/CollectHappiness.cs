using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHappiness : CollectItem<HappinessHolder> {
		
		public override EnabledState EnabledState {
			get { return new DefaultEnabledState (); }
		}
	}
}