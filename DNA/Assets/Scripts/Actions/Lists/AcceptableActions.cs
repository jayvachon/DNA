using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class AcceptableActions : ActionList<AcceptorAction> {

		new public AcceptorAction this[string id] {
			get { return ActiveActions[id] as AcceptorAction; }
		}

		IActionAcceptor acceptor;

		public AcceptableActions (IActionAcceptor acceptor) {
			this.acceptor = acceptor;
		}

		public void Add (AcceptorAction action) {
			action.Acceptor = acceptor;
			AddAction (action);
		}
	}
}