using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class LaborGroup : ItemGroup<LaborItem> {

		public override string ID {
			get { return "Labor"; }
		}

		public LaborGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}