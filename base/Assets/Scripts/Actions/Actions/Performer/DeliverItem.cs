using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : PerformerAction where T : ItemHolder {

		public override string Name {
			get { return "Deliver " + (typeof (T)); }
		}

		public override bool Enabled {
			get { return Holder.Count > 0; }
		}

		public IActionAcceptor Acceptor { get; set; }

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

		Inventory AcceptorInventory {
			get {
				IBinder binder = Performer as IBinder;
				IInventoryHolder holder = binder.BoundAcceptor as IInventoryHolder;
				return holder.Inventory;
			}
		}

		public DeliverItem (float duration) : base (duration) {}
	
		public override void End () {
			AcceptorInventory.Transfer<T> (Inventory, 1);
			base.End ();
		}		
	}
}