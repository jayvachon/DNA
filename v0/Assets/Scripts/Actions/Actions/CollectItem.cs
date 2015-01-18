using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class CollectItem<T> : Action where T : ItemHolder {

		Inventory receiver, sender;
		int transferAmount;

		public CollectItem (Inventory receiver, int transferAmount, float duration) : base ("Collect Item", duration) {
			this.receiver = receiver;
			this.transferAmount = transferAmount;
		}

		public override void Start (IActionAcceptor acceptor) {
			IInventoryHolder holder = acceptor as IInventoryHolder;
			sender = holder.Inventory;
			if (CanTransfer ()) {
				base.Start ();
			} else {
				base.End ();
			}
		}

		public override void End () {
			receiver.Transfer<T> (sender, transferAmount);
			base.End ();
		}

		protected virtual bool CanTransfer () {
			return !receiver.Get<T> ().Full;
		}
	}
}