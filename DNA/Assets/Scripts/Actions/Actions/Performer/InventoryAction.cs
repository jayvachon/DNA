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
		protected virtual Inventory Inventory {
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

		protected virtual Inventory AcceptorInventory {
			get {
				IBinder binder = Performer as IBinder;
				IInventoryHolder holder = binder.BoundAcceptor as IInventoryHolder;
				if (holder == null) {
					return null;
				} else {
					return holder.Inventory;
				}
			}
		}

		bool listeningForUpdate = false;
		public override bool Enabled {
			get {
				if (!ListenForUpdate) {
					return EnabledState.Enabled;
				}
				if (!EnabledState.Enabled && !listeningForUpdate) {
					holder.HolderUpdated += Start;
					listeningForUpdate = true;
				} else if (listeningForUpdate) {
					holder.HolderUpdated -= Start;
					listeningForUpdate = false;
				}
				return EnabledState.Enabled;
			}
		}

		public virtual bool ListenForUpdate {
			get { return true; }
		}

		public InventoryAction (float duration=-1, bool autoStart=false, bool autoRepeat=false) : 
			base (duration, autoStart, autoRepeat) {}
	}
}
