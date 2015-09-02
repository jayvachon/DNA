using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class CollectItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Full; }
		}

		protected override void OnEnd () {
			Inventory.Transfer<T> (AcceptorInventory, 1);
			base.OnEnd ();
		}
	}
}