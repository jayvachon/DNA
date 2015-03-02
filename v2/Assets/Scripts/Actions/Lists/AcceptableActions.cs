using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class AcceptableActions : ActionList<AcceptorAction> {

		IActionAcceptor acceptor;

		public AcceptableActions (IActionAcceptor acceptor) {
			this.acceptor = acceptor;
		}

		public void Add (string id, AcceptorAction action) {
			action.Acceptor = acceptor;
			AddAction (id, action);
		}

		public void Bind (IActionPerformer boundPerformer) {
			IInventoryHolder holder = boundPerformer as IInventoryHolder;
			Inventory boundInventory = holder.Inventory;
			foreach (var action in Actions) {
				AcceptorAction acceptorAction = action.Value as AcceptorAction;
				acceptorAction.Bind (boundInventory);
			}
		}
	}
}