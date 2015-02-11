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

		public bool HasSick () {
			foreach (ElderItem elder in Items) {
				if (elder.Health < 0.5f) {
					return true;
				}
			}
			return false;
		}

		public bool HasHealthy () {
			foreach (ElderItem elder in Items) {
				if (elder.Health >= 0.5f) {
					return true;
				}
			}
			return false;
		}
	}
}