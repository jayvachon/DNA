using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class AcceptorAction : Action {

		public IActionAcceptor Acceptor { get; set; }
		
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
	}
}