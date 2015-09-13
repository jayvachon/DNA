using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public class YearHolder : ItemHolder<YearItem> {

		public override string Name {
			get { return "Years"; }
		}

		public YearHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}
