using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class MilkshakeHolder : ItemHolder<MilkshakeItem> {

		public MilkshakeHolder (int capacity=5) : base ("Milkshake", capacity) {} 
	}
}
