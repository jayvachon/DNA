using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
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
	}

	public abstract class AcceptInventoryTask<T> : AcceptInventoryTask where T : ItemGroup {//ItemHolder {

		/*T holder = null;
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
		}*/

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