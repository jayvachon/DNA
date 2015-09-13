using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class AcceptCollectItem<T> : AcceptInventoryTask<T> where T : ItemHolder {

		public override System.Type AcceptedTask {
			get { return typeof (CollectItem<T>); }
		}
		
		public override bool Enabled {
			get { return !Holder.Empty; }
		}

		public AcceptCollectItem (Inventory inventory=null) : base (inventory) {}
	}
}