using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class HappinessHolder : ItemHolder<HappinessItem> {

		public override string Name {
			get { return "Happiness"; }
		}

		public HappinessHolder (int capacity, int startCount) : base (capacity, startCount) {}

	}
}