using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class MilkHolder : ItemHolder<MilkItem> {

		public override string Name {
			get { return "Milk"; }
		}

		public MilkHolder (int capacity, int startCount) : base (capacity, startCount) {}

	}
}