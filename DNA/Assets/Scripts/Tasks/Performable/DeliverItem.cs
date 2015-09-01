using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class DeliverItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Empty; }
		}

		AcceptDeliverItem<T> accept;

		public void Start (AcceptDeliverItem<T> accept) {
			if (Start ()) this.accept = accept;
		}
		
		protected override void OnEnd () {
			accept.Inventory.Transfer<T> (Inventory, 1);
			base.OnEnd ();
		}
	}
}