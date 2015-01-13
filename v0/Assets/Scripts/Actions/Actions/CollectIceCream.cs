using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectIceCream : Action {

		Inventory receiver;
		Inventory sender;
		int transferAmount = 1;

		public CollectIceCream (Inventory receiver, int transferAmount, float duration=2) : base (duration) {
			this.receiver = receiver;
			this.transferAmount = transferAmount;
		}

		public override void Start (IActionAcceptor acceptor) {
			IInventoryHolder holder = acceptor as IInventoryHolder;
			sender = holder.Inventory;
			if (!receiver.Get<IceCreamHolder> ().Full && sender.Has<IceCreamHolder> ()) {
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