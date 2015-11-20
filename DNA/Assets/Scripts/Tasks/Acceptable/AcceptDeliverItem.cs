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

		protected override int PerformCount (Inventory i) {
			if (Group.HasCapacity) {
				return Mathf.Min (i.Get<T> ().Count, Group.Capacity - Group.Count);
			} else {
				return i.Get<T> ().Count;
			}
		}
	}
}