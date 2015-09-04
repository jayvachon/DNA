using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {
	
	public class AcceptCollectItemTest<T> : AcceptCollectItem<T> where T : ItemHolder {
		
		public override System.Type AcceptedTask {
			get { return typeof (CollectItemTest<T>); }
		}

		public AcceptCollectItemTest (Inventory inventory=null) : base (inventory) {}
	}
}