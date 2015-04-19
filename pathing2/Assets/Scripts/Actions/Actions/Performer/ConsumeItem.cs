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
					name = "Consume " + typeName;
				}
				return name;
			}
		}

		public ConsumeItem (float duration, bool autoStart=true, bool autoRepeat=true, bool enableable=true) : base (duration, autoStart, autoRepeat, null) {
			this.enableable = enableable;
		}
		
		public override void OnEnd () {
			Holder.Remove ();
		}
	}
}