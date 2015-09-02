using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class InventoryTask<T> : PerformerTask where T : ItemHolder {

		Inventory inventory = null;
		protected virtual Inventory Inventory {
			get {
				if (inventory == null && Performer is IInventoryHolder) {
					IInventoryHolder holder = Performer as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					throw new System.Exception (string.Format ("TaskPerformer {0} does not implement IInventoryHolder", Performer));
				}
				return inventory;
			}
		}

		T holder = null;
		protected T Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<T> ();
				}
				if (holder == null) {
					throw new System.Exception ("Inventory does not include " + typeof (T));
				}
				return holder;
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
	}
}