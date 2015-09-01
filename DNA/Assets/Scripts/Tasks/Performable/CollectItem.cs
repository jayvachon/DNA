using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class CollectItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Full; }
		}

		AcceptCollectItem<T> accept;

		public void Start (AcceptCollectItem<T> accept) {
			if (Start ()) this.accept = accept;
		}

		protected override void OnEnd () {
			Inventory.Transfer<T> (accept.Inventory, 1);
			base.OnEnd ();
		}
	}
}