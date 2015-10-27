using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class AcceptCollectItem<T> : AcceptInventoryTask<T> where T : ItemGroup {

		public override System.Type AcceptedTask {
			get { return typeof (CollectItem<T>); }
		}
		
		public override bool Enabled {
			get { return !Group.Empty; }
		}

		public AcceptCollectItem (Inventory inventory=null) : base (inventory) {}
	}
}