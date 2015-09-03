using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class AcceptCollectItem<T> : AcceptInventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Empty; }
		}

		public AcceptCollectItem (Inventory inventory=null) : base (inventory) {}
	}
}