using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class ConsumeItem<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Consume" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new ConsumeItemEnabledState (Holder);
				}
				return enabledState;
			}
		}

		public ConsumeItem (float duration=-1, bool autoStart=true, bool autoRepeat=true) : base (duration, autoStart, autoRepeat) {}
		
		public override void OnEnd () {
			Holder.Remove ();
		}
	}
}