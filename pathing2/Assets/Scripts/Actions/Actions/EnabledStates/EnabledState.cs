
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public abstract class EnabledState {

		public abstract bool Enabled { get; }

		public virtual string RequiredPair {
			get { return ""; }
		}

		public virtual bool RequiresPair {
			get { return RequiredPair == ""; }
		}

		// TODO: Make protected after finished testing
		public bool Paired { get; private set; }

		public Inventory BoundInventory { get; set; }

		public bool AttemptPair (List<IActionAcceptor> acceptors) {
			
			if (RequiredPair == "") {
				return false;
			}

			foreach (IActionAcceptor acceptor in acceptors) {
				if (acceptor.AcceptableActions.Has (RequiredPair)) {
					Paired = true;
					return true;
				}
			}
			Paired = false;
			return false;
		}
	}
}