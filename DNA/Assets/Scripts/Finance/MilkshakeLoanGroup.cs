using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA {

	public class MilkshakeLoanGroup : ItemGroup<Loan<MilkshakeGroup>> {

		public override string ID {
			get { return "Milkshakes"; }
		}

		public MilkshakeLoanGroup (int capacity) : base (0, capacity) {}
	}
}