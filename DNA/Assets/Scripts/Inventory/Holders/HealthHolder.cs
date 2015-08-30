using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class HealthHolder : ItemHolder<HealthItem> {

		public override string Name {
			get { return "Health"; }
		}

		public HealthHolder (int capacity, int startCount) : base (capacity, startCount) {}

	}
}