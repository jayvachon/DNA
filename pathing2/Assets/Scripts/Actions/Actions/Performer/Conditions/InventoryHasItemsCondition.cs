using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class InventoryHasItemsCondition : PerformCondition {

		public override bool Performable {
			get {
				for (int i = 0; i < holders.Length; i ++) {
					if (holders[i].Empty)
						return false;
				}
				return true;
			}
		}

		ItemHolder[] holders;

		public InventoryHasItemsCondition (Inventory inventory, string[] holderNames) {
			holders = new ItemHolder[holderNames.Length];
			for (int i = 0; i < holders.Length; i ++) {
				holders[i] = inventory.Get (holderNames[i]);
			}
		}
	}
}