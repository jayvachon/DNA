﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectHealthEnabledState : CollectItemEnabledState<HealthHolder> {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		public override string RequiredPair {
			get { return ""; }
		}

		public CollectHealthEnabledState (ItemHolder holder) : base (holder) {}
	}
}