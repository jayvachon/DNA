using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA {

	public class CoffeeLoanGroup : ItemGroup<Loan<CoffeeGroup>> {

		public override string ID {
			get { return "Coffee"; }
		}

		public CoffeeLoanGroup (int capacity) : base (0, capacity) {}
	}
}