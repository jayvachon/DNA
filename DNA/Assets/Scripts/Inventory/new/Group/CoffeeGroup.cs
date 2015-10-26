using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class CoffeeGroup : ItemGroup<CoffeeItem> {

		public override string ID {
			get { return "Coffee"; }
		}

		public CoffeeGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}