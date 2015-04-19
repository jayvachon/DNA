using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptInventoryAction<T> : AcceptorAction where T : ItemHolder {

		ItemHolder holder = null;
		protected ItemHolder Holder {
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

		public AcceptInventoryAction (AcceptCondition acceptCondition) : base (acceptCondition) {}
	
	}
}
