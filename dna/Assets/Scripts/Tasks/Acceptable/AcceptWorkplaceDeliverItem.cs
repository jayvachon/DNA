using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class AcceptWorkplaceDeliverItem<T> : AcceptDeliverItem<T> where T : ItemGroup {

		public override System.Type AcceptedTask {
			get { return typeof (WorkplaceDeliverItem<T>); }
		}

		public AcceptWorkplaceDeliverItem (Inventory inventory=null) : base (inventory) {}
		
	}
}