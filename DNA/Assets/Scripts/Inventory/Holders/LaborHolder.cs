using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {

	public class LaborHolder : ItemHolder<LaborItem> {

		public override string Name {
			get { return "Labor"; }
		}

		public LaborHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}