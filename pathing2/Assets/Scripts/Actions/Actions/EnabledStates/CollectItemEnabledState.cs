using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectItemEnabledState<T> : EnabledState where T : ItemHolder {

		public override bool Enabled {
			get { return !holder.Full; }
		}

		string requiredPair = "";
		public override string RequiredPair {
			get { 
				if (requiredPair == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					requiredPair = "Deliver" + typeName;
				}
				return requiredPair;
			}
		}

		public override bool RequiresPair {
			get { return true; }
		}

		protected ItemHolder holder;

		public CollectItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}