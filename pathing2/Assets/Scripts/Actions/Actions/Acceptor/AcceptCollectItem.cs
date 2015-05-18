using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectItem<T> : AcceptInventoryAction<T> where T : ItemHolder {
		
		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Collect" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new AcceptCollectItemEnabledState<T> (Holder);
				}
				return enabledState;
			}
		}
	}
}