using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class HealthGroup : ItemGroup<HealthItem> {

		public override string ID {
			get { return "Health"; }
		}

		public HealthGroup (int startCount=0, int capacity=-1) : base (startCount, capacity) {}
	}
}