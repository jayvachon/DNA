using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public abstract class AcceptInventoryTask : AcceptorTask {

		protected Inventory inventory = null;
		public Inventory Inventory {
			get {
				if (inventory == null && Acceptor is IInventoryHolder) {
					IInventoryHolder holder = Acceptor as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					throw new System.Exception ("TaskAcceptor does not implement IInventoryHolder");
				}
				return inventory;
			}
		}

		public int GetPerformCount (PerformerTask p) {
			IInventoryHolder h = p.Performer as IInventoryHolder;
			if (h == null)
				return 0;

			return PerformCount (h.Inventory);
		}

		protected virtual int PerformCount (Inventory i) { return 0; }
	}

	public abstract class AcceptInventoryTask<T> : AcceptInventoryTask where T : ItemGroup {

		T group = null;
		protected T Group {
			get {
				if (group == null) {
					group = Inventory.Get<T> ();
				}
				if (group == null) {
					throw new System.Exception ("Inventory does not include " + typeof (T));
				}
				return group;
			}
		}

		public AcceptInventoryTask (Inventory inventory=null) {
			this.inventory = inventory;
		}
	}
}