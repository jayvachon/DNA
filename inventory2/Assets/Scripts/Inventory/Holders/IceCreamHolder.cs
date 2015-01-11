using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class IceCreamHolder : ItemHolder<IceCreamItem> {

		public IceCreamHolder (int capacity=5) : base (capacity) {}
		
		public List<IceCreamItem> RemoveFlavor (Flavor flavor) {
			return RemoveFlavor (flavor, Capacity);
		}

		public List<IceCreamItem> RemoveFlavor (Flavor flavor, int amount) {
			List<IceCreamItem> temp = new List<IceCreamItem> ();
			List<IceCreamItem> matching = new List<IceCreamItem> ();
			foreach (IceCreamItem item in items) {
				if (item.Flavor == flavor && matching.Count < amount) {
					matching.Add (item);
				} else {
					temp.Add (item);
				}
			}
			items = temp;
			return matching;
		}

		public void TransferFlavor (Inventory boundInventory, Flavor flavor, int amount=-1) {
			if (amount == -1) amount = Capacity;
			IceCreamHolder sender = boundInventory.Get<IceCreamHolder> () as IceCreamHolder;
			List<Item> items = ToItemsList<IceCreamItem> (sender.RemoveFlavor (flavor, amount));
			List<Item> overflow = Add (items);
			sender.Add (overflow);
		}

		public override void Print () {
			foreach (IceCreamItem item in items) {
				Debug.Log (item.Flavor);
			}
		}
	}
}