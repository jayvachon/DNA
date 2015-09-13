using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public class CoffeeHolder : ItemHolder<CoffeeItem> {

		public override string Name {
			get { return "Coffee"; }
		}

		public CoffeeHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}
