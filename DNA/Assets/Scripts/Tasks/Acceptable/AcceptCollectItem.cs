using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class AcceptCollectItem<T> : AcceptInventoryTask<T> where T : ItemGroup {

		public override System.Type AcceptedTask {
			get { return typeof (CollectItem<T>); }
		}
		
		public override bool Enabled {
			get { return !Group.AtMinimum; }
		}

		public AcceptCollectItem (Inventory inventory=null) : base (inventory) {}

		protected override int PerformCount (Inventory i) {
			T performerGroup = i.Get<T> ();
			if (performerGroup.HasCapacity) {
				return Mathf.Min (Group.Count, performerGroup.Capacity - performerGroup.Count);
			} else {
				return Group.Count;
			}
		}
	}
}