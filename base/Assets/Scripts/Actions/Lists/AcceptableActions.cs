using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	}
}