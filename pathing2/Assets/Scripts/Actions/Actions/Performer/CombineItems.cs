using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	// not needed ? 

	// Item<T> + Item<U> = Item<V>
	public class CombineItems<T, U, V> : InventoryAction<T> where T: ItemHolder where U: ItemHolder where V: ItemHolder {

		string name = "";
		public override string Name {
			get {
				if (name == "") {
					string typeName = typeof (T).Name;
					string typeName2 = typeof (U).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					typeName2 = typeName2.Substring (0, typeName2.Length-6);
					name = "Combine" + typeName + "And" + typeName2;
				}
				return name;
			}
		}

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

		public CombineItems (float duration) : base (duration, true, true) {}

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