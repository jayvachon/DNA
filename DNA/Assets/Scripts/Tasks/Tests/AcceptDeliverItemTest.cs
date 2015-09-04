using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class AcceptDeliverItemTest<T> : AcceptDeliverItem<T> where T : ItemHolder {

		public override System.Type AcceptedTask {
			get { return typeof (DeliverItemTest<T>); }
		}

		public AcceptDeliverItemTest (Inventory inventory=null) : base (inventory) {}
	}
}