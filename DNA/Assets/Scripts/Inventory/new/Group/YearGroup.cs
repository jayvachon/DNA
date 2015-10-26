using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class YearGroup : ItemGroup<YearItem> {

		public override string ID {
			get { return "Years"; }
		}

		public YearGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}