using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class MilkshakeGroup : ItemGroup<MilkshakeItem> {

		public override string ID {
			get { return "Milkshakes"; }
		}

		public MilkshakeGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}