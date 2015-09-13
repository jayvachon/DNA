using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public delegate void MilkshakeHolderFull ();

	public class MilkshakeHolder : ItemHolder<MilkshakeItem> {

		public override string Name {
			get { return "Milkshakes"; }
		}

		public MilkshakeHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}
