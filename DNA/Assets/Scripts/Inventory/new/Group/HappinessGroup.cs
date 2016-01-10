using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class HappinessGroup : ItemGroup<HappinessItem> {

		public override string ID {
			get { return "Happiness"; }
		}

		public HappinessGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}