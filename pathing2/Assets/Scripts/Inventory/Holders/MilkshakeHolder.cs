using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public delegate void MilkshakeHolderFull ();

	public class MilkshakeHolder : ItemHolder<MilkshakeItem> {

		public override string Name {
			get { return "Milkshakes"; }
		}

		MilkshakeHolderFull holderFull;
		public MilkshakeHolderFull HolderFull { get; set; }

		public MilkshakeHolder (int capacity, int startCount, MilkshakeHolderFull holderFull=null) : base (capacity, startCount) {
			this.holderFull = holderFull;
		}

		public override void OnTransfer () {
			if (holderFull != null && Full) holderFull ();
		}
	}
}
