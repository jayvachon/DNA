using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {
	public class AcceptDeliverToPlayer<T> : AcceptDeliverItem<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "DeliverToPlayer" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptDeliverToPlayerEnabledState<T> (Holder);
				}
				return enabledState;
			}
		}
	}
}