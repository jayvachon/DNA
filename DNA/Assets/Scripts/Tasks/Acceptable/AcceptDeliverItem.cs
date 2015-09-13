using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class AcceptDeliverItem<T> : AcceptInventoryTask<T> where T : ItemHolder {

		public override System.Type AcceptedTask {
			get { return typeof (DeliverItem<T>); }
		}

		public override bool Enabled {
			get { return !Holder.Full; }
		}	

		public AcceptDeliverItem (Inventory inventory=null) : base (inventory) {}
	}
}