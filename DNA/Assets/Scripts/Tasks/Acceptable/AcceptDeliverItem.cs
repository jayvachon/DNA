using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class AcceptDeliverItem<T> : AcceptInventoryTask<T> where T : ItemGroup {

		public override System.Type AcceptedTask {
			get { return typeof (DeliverItem<T>); }
		}

		public override bool Enabled {
			get { return !Group.Full; }
		}	

		public AcceptDeliverItem (Inventory inventory=null) : base (inventory) {}
	}
}