
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

		protected bool Paired { get; private set; }

		public Inventory BoundInventory { get; set; }

		public bool AttemptPair (IActionAcceptor acceptor) {
			return acceptor.AcceptableActions.Has (RequiredPair);
		}
	}
}