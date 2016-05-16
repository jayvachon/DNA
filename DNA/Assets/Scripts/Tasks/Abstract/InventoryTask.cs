using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public abstract class InventoryTask : PerformerTask {

		Inventory inventory = null;
		protected virtual Inventory Inventory {
			get {
				if (inventory == null && Performer is IInventoryHolder) {
					IInventoryHolder holder = Performer as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					if (Performer == null) {
						throw new System.Exception ("'Performer' has not been set for the InventoryTask '" + this + "'");
					}
					throw new System.Exception (string.Format ("TaskPerformer {0} does not implement IInventoryHolder", Performer));
				}
				return inventory;
			}
		}

		public InventoryTask (string symbolOverride) : base (symbolOverride) {}
		public InventoryTask (Inventory inventory=null) {
			this.inventory = inventory;
		}
	}

	public abstract class InventoryTask<T> : InventoryTask where T : ItemGroup {

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

		protected Inventory AcceptorInventory {
			get {
				try { 
					AcceptInventoryTask<T> task = (AcceptInventoryTask<T>)acceptTask;
					return task.Inventory;
				}
				catch {
					throw new System.Exception ("No acceptor task in '" + this + "' or acceptor task is not type AcceptInventoryTask<" + typeof (T) + "'");
				}
			}
		}

		public InventoryTask (string symbolOverride) : base (symbolOverride) {}
		public InventoryTask (Inventory inventory=null) : base (inventory) {}
	}
}