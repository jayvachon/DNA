using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectItem<T> : PerformerAction where T : ItemHolder {

		public override string Name {
			get { return "Collect " + (typeof (T)); }
		}

		public override bool Enabled {
			get { return !Holder.Full; }
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

		public CollectItem (float duration) : base (duration) {}
		
		public override void End () {
			Inventory.Transfer<T> (AcceptorInventory, 1);
			base.End ();
		}
	}
}