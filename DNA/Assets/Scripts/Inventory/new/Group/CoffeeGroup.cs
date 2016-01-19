using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class CoffeeGroup : ItemGroup<CoffeeItem> {

		public override string ID {
			get { return "Coffee"; }
		}

		public CoffeeGroup () : base (0, -1) {}
		public CoffeeGroup (int startCount, int capacity=-1) : base (startCount, capacity) {}
	}
}