using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Deliver" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new DeliverItemEnabledState<T> (Holder);
				}
				return enabledState;
			}
		}

		public IActionAcceptor Acceptor { get; set; }

		public override void OnEnd () {
			// AcceptorInventory.Transfer<T> (Inventory, 1, AcceptCondition.Transferable);
			AcceptorInventory.Transfer<T> (Inventory, 1);
		}		
	}
}