using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class YearGroup : ItemGroup<YearItem> {

		public override string ID {
			get { return "Years"; }
		}

		public YearGroup (int capacity, int startCount) : base (capacity, startCount) {}
	}
}