using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class AcceptCondition {
		
		public abstract bool Acceptable { get; }
		public Inventory Inventory { get; set; }
		public Inventory PerformerInventory { get; set; }
	}
}