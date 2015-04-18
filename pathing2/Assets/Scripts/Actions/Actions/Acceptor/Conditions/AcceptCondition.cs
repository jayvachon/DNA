using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class AcceptCondition {

		// This is checked before any time is spent carrying out the Action
		// if false is returned, the Acceptor is skipped over
		public abstract bool CanAccept { get; }

		// This is checked for individual inventory items during the transfer
		public virtual ItemHasAttribute Transferable {
			get { return null; }
		}

		// The Acceptor's inventory
		public Inventory Inventory { get; set; }

		// The Performer's inventory
		public Inventory PerformerInventory { get; set; }
	}
}