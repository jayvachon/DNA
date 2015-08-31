using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverToPlayer<T> : DeliverItem<T> where T : ItemHolder {

		/*string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "DeliverToPlayer" + typeName;
				}
				return name;
			}
		}*/

		/*EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new DeliverItemEnabledState<T> (Holder);
				}
				return enabledState;
			}
		}*/

		protected override Inventory AcceptorInventory {
			get { return Player.Instance.Inventory; }
		}

		public DeliverToPlayer (float duration=-1, bool autoStart=false, bool autoRepeat=false) 
			: base (duration, autoStart, autoRepeat) {}
	}
}