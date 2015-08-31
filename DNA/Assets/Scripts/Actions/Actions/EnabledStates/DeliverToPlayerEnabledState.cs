using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverToPlayerEnabledState<T> : EnabledState where T : ItemHolder {

		public override bool Enabled {
			get { return !holder.Empty; }
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

		public DeliverToPlayerEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}