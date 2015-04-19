using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class InventoryAction<T> : PerformerAction where T : ItemHolder {

		T holder = null;
		protected T Holder {
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

		Inventory inventory = null;
		protected Inventory Inventory {
			get {
				if (inventory == null && Performer is IInventoryHolder) {
					IInventoryHolder holder = Performer as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					Debug.LogError (string.Format ("ActionPerformer {0} does not implement IInventoryHolder", Performer));
				}
				return inventory;
			}
		}

		protected Inventory AcceptorInventory {
			get {
				IBinder binder = Performer as IBinder;
				IInventoryHolder holder = binder.BoundAcceptor as IInventoryHolder;
				return holder.Inventory;
			}
		}

		protected T AcceptorHolder {
			get { return AcceptorInventory.Get<T> (); }
		}

		public InventoryAction (float duration, bool autoStart=false, bool autoRepeat=false, PerformCondition performCondition=null) : base (duration, autoStart, autoRepeat, performCondition) {}
	}
}
