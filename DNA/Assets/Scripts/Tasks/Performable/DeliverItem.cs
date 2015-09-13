using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class DeliverItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Empty; }
		}
		
		protected override void OnEnd () {
			AcceptorInventory.Transfer<T> (Inventory, 1);
			base.OnEnd ();
		}
	}
}