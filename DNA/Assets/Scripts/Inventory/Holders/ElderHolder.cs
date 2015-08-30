using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class ElderHolder : ItemHolder<ElderItem> {

		public ElderHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}