using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : Action where T : ItemHolder {

		Inventory receiver, sender;
		int transferAmount;

		public DeliverItem (Inventory sender, int transferAmount, float duration) : base ("Deliver Item", duration) {
			this.sender = sender;
			this.transferAmount = transferAmount;
		}

		public override void Start (IActionAcceptor acceptor) {
			IInventoryHolder holder = acceptor as IInventoryHolder;
			receiver = holder.Inventory;
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
			return !receiver.Get<T> ().Full && !sender.Get<T> ().Empty;
		}
	}
}