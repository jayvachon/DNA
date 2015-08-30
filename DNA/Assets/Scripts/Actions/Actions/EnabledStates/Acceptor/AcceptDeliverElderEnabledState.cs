using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	// TODO: Deprecate
	public class AcceptDeliverElderEnabledState : EnabledState {

		public override bool Enabled {
			get { return !holder.Full; }
		}
		
		string requiredPair = "";
		public override string RequiredPair {
			get { 
				if (requiredPair == "") {
					string typeName = typeof (ElderItem).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					requiredPair = "Deliver" + typeName;
				}
				return requiredPair;
			}
		}

		ItemHolder holder;

		public AcceptDeliverElderEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}