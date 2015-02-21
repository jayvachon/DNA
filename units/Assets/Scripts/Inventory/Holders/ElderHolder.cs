using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public class ElderHolder : ItemHolder<ElderItem> {

		public override string Name {
			get { return "Elders"; }
		}
		
		public ElderHolder (int capacity, int startCount) : base (capacity) {
			AddNew (startCount);
		}

		void AddNew (int count) {
			if (count == 0)
				return;
			for (int i = 0; i < count; i ++) {
				Add (new ElderItem ());
			}
		}

		public override void Print () {
			foreach (ElderItem item in Items) {
				ElderItem elder = item as ElderItem;
				elder.Print ();
			}
		}
	}
}