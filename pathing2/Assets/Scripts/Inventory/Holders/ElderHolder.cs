using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public class ElderHolder : ItemHolder<ElderItem> {

		public ElderHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}