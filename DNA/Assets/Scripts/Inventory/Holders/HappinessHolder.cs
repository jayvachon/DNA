using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InventorySystem {
	
	public class HappinessHolder : ItemHolder<HappinessItem> {

		public override string Name {
			get { return "Happiness"; }
		}

		public int Average {
			get {
				int count = history.Count;
				int sum = 0;
				for (int i = 0; i < count; i ++) {
					sum += history[i];
				}
				return Mathf.RoundToInt ((float)sum / (float)count);
			}
		}

		List<int> history = new List<int> ();
		int prevCount;

		public HappinessHolder (int capacity, int startCount) : base (capacity, startCount) {
			history.Add (startCount);
			prevCount = startCount;
		}

		public override void Initialize (int count) {
			base.Initialize (count);
			history.Clear ();
			history.Add (count);
			prevCount = count;
		}

		public override void OnHolderUpdated () {
			if (Count < prevCount) {
				history.Add (Count);
			}
			prevCount = Count;
		}
	}
}