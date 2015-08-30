using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverAllItemsEnabledState<T> : EnabledState where T : ItemHolder {

		public override bool Enabled {
			get { return Paired && !holder.Empty; }
		}
		
		string requiredPair = "";
		public override string RequiredPair {
			get { 
				if (requiredPair == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					requiredPair = "DeliverAll" + typeName;
				}
				return requiredPair;
			}
		}

		ItemHolder holder;

		public DeliverAllItemsEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}