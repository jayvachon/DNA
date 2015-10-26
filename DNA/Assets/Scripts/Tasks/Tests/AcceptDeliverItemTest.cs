using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class AcceptDeliverItemTest<T> : AcceptDeliverItem<T> where T : ItemGroup {

		public override System.Type AcceptedTask {
			get { return typeof (DeliverItemTest<T>); }
		}

		public AcceptDeliverItemTest (Inventory inventory=null) : base (inventory) {}
	}
}