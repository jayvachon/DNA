using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class DistributorHolder : ItemHolder<DistributorItem> {

		public override string Name { get { return "Laborers"; } }

		public DistributorHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}