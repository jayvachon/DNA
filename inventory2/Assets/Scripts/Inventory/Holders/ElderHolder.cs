using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameInventory {

	public class ElderHolder : ItemHolder<ElderItem> {

		public ElderHolder (int capacity=5) : base (capacity) {}
		
	}
}