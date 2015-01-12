using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectIceCream : Action {

		Inventory receiver;
		Inventory sender;
		int transferAmount = -1;

		public CollectIceCream (Inventory receiver, float duration=2) : base (duration) {
			this.receiver = receiver;
		}

		public override void Start (object[] args) {
			this.sender = args[0] as Inventory;
			if (args.Length > 0)
				this.transferAmount = (int)args[1];
			base.Start ();
		}

		public override void End () {
			receiver.Transfer<IceCreamHolder> (sender, transferAmount);
			base.End ();
		}
	}
}