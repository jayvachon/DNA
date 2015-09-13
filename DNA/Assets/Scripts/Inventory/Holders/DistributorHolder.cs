using UnityEngine;
using System.Collections;

namespace DNA.InventorySystem {

	public class DistributorHolder : ItemHolder<DistributorItem> {

		public override string Name { get { return "Laborers"; } }

		public DistributorHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}