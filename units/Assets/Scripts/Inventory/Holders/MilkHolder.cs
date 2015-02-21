using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class MilkHolder : ItemHolder<MilkItem> {

		public override string Name {
			get { return "Milk"; }
		}

		public MilkHolder (int capacity, int startCount) : base (capacity) {
			AddNew (startCount);
		}

		void AddNew (int count) {
			if (count == 0)
				return;
			for (int i = 0; i < count; i ++) {
				Add (new MilkItem ());
			}
		}
	}
}