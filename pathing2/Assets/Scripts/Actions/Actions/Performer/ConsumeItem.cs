using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class ConsumeItem<T> : PerformerAction where T : ItemHolder {

		ItemHolder holder = null;
		ItemHolder Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<T> ();
				}
				if (holder == null) {
					Debug.LogError ("Inventory does not include " + typeof (T));
				}
				return holder;
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