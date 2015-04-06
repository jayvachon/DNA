using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectItem<T> : PerformerAction where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Collect " + typeName;
				}
				return name;
			}
		}

		public override System.Type RequiredPair {
			get { return typeof (DeliverItem<T>); }
		}

		public override bool CanPerform {
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
		
		public override void OnEnd () {
			Inventory.Transfer<T> (AcceptorInventory, 1, AcceptCondition.Transferable);
		}
	}
}