using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptInventoryAction<T> : AcceptorAction where T : ItemHolder {

		Inventory inventory = null;
		protected Inventory Inventory {
			get {
				if (inventory == null && Acceptor is IInventoryHolder) {
					IInventoryHolder holder = Acceptor as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					Debug.LogError ("ActionAcceptor does not implement IInventoryHolder");
				}
				return inventory;
			}
		}
		
		ItemHolder holder = null;
		protected ItemHolder Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<T> ();
				}
				if (holder == null) {
					Debug.LogError ("Inventory does not include " + typeof (T));
				}
				return holder;
			}
		}
	}
}
