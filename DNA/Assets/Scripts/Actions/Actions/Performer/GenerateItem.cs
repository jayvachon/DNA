using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItem<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Generate" + typeName;
				}
				return name;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new GenerateItemEnabledState (Holder);
				}
				return enabledState;
			}
		}

		public GenerateItem () : base (-1f, true, true) {}

		public override void OnEnd () {
			Holder.Add ();
		}
	}
}