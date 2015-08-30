using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverItemEnabledState<T> : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		string requiredPair = "";
		public override string RequiredPair {
			get { 
				if (requiredPair == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					requiredPair = "Collect" + typeName;
				}
				return requiredPair;
			}
		}

		public override bool RequiresPair {
			
			// Not super elegant, but this works
			// The acceptor only requires a pair if the performer does not have any coffee
			get { return BoundInventory["Coffee"].Count == 0; }
		}

		ItemHolder holder;

		public AcceptDeliverItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}