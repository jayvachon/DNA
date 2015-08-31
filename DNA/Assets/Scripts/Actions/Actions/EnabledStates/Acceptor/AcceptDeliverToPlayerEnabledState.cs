using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverToPlayerEnabledState<T> : EnabledState {

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
			get { return true; }
		}

		ItemHolder holder;

		public AcceptDeliverToPlayerEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}