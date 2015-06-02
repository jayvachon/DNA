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
		public override bool ListenForUpdate { get { return false; } }

		public DeliverItem (float duration=-1, bool autoStart=false, bool autoRepeat=false) 
			: base (duration, autoStart, autoRepeat) {}

		public override void OnEnd () {
			if (AcceptorInventory != null)
				AcceptorInventory.Transfer<T> (Inventory, 1);
		}		
	}
}