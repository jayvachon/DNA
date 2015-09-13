using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public class BedHolder : ItemHolder<BedItem> {

		public override string Name {
			get { return "Bed"; }
		}

		float quality = 0f;
		public float Quality { get { return quality; } }

		public BedHolder (int capacity, int startCount, float quality) : base (capacity, startCount) {
			this.quality = quality;
		}
	}
}