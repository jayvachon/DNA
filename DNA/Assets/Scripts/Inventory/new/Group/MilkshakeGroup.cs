using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class MilkshakeGroup : ItemGroup<MilkshakeItem> {

		public override string ID {
			get { return "Milkshakes"; }
		}

		public MilkshakeGroup () : base (0, -1) {}
		public MilkshakeGroup (int startCount, int capacity=-1) : base (startCount, capacity) {}
	}
}