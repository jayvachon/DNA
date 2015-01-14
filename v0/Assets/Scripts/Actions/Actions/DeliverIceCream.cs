using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverIceCream : Action {

		Inventory receiver;
		Inventory sender;
		int transferAmount;

		public DeliverIceCream (Inventory sender, int transferAmount, float duration=2) : base (duration) {
			this.sender = sender;
			this.transferAmount = transferAmount;
		}

		public override void Start (IActionAcceptor acceptor) {
			IInventoryHolder holder = acceptor as IInventoryHolder;
			receiver = holder.Inventory;
			if (!receiver.Get<IceCreamHolder> ().Full && !sender.Get<IceCreamHolder> ().Empty) {
				base.Start ();
			} else {
				base.End ();
			}
		}

		public override void End () {
			receiver.Transfer<IceCreamHolder> (sender, transferAmount);
			base.End ();
		}
	}
}