﻿using UnityEngine;
using System.Collections;
using GameInventory;
using Units;

namespace GameActions {

	public class DeliverYear : DeliverItem<YearHolder> {

		public override EnabledState EnabledState {
			get { return new DefaultEnabledState (); }
		}
	}
}