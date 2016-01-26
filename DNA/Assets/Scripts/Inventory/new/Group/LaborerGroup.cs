using UnityEngine;
using System.Collections;

namespace InventorySystem {

	public class LaborerGroup : ItemGroup<LaborerItem> {

		public override string ID {
			get { return "Laborer"; }
		}

		public LaborerGroup (int capacity) : base (0, capacity) {}
	}
}