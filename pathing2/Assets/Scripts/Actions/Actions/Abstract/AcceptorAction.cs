using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class AcceptorAction : Action {

		public IActionAcceptor Acceptor { get; set; }

		public void Bind (Inventory inventory) {
			EnabledState.BoundInventory = inventory;
		}
	}
}