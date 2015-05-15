using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverAllItems<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "DeliverAll" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new DeliverAllItemsEnabledState<T> (Holder);
				}
				return enabledState;
			}
		}

		public IActionAcceptor Acceptor { get; set; }

		public DeliverAllItems (float duration=0, bool autoStart=false, bool autoRepeat=false) 
			: base (duration, autoStart, autoRepeat) {}

		public override void OnEnd () {
			AcceptorInventory.Transfer<T> (Inventory, -1);
		}		
	}
}