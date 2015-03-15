using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	// T + U = V
	public class CombineItems<T, U, V> : PerformerAction where T: ItemHolder where U: ItemHolder where V: ItemHolder {

		ItemHolder holdert = null;
		ItemHolder HolderT {
			get {
				if (holdert == null) {
					holdert = Inventory.Get<T> ();
				}
				return holdert;
			}
		}

		ItemHolder holderu = null;
		ItemHolder HolderU {
			get {
				if (holderu == null) {
					holderu = Inventory.Get<U> ();
				}
				return holderu;
			}
		}

		ItemHolder holderv = null;
		ItemHolder HolderV {
			get {
				if (holderv == null) {
					holderv = Inventory.Get<V> ();
				}
				return holderv;
			}
		}

		public CombineItems (float duration) : base (duration, true, true, null) {}

		public override void OnEnd () {
			if (!HolderT.Empty && !HolderU.Empty) {
				HolderT.Remove ();
				HolderU.Remove ();
				HolderV.Add ();
				Inventory.NotifyInventoryUpdated ();
			}
		}
	}
}