using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public class HealthHolder : ItemHolder<HealthItem> {

		public override string Name {
			get { return "Health"; }
		}

		public HealthHolder (int capacity, int startCount) : base (capacity, startCount) {}

	}
}