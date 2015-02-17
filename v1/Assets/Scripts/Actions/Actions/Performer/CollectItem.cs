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
		
		public override void OnEnd () {
			// TODO: brute forcing it as a proof of concept
			// the way to do this would be to use a lambda
			// (but this IS working)
			if (AcceptCondition is ElderCondition) {
				ElderCondition condition = AcceptCondition as ElderCondition;
				ElderHolder sender = AcceptorInventory.Get<ElderHolder> () as ElderHolder;
				ElderHolder receiver = Inventory.Get<ElderHolder> () as ElderHolder;
				if (condition.requestSick) {
					receiver.TransferSick (sender, 1);
				} else {
					receiver.TransferHealthy (sender, 1);
				}
				Debug.Log ("Collector's inventory:");
				Inventory.Print ();
				Debug.Log ("Static unit's inventory");
				AcceptorInventory.Print ();
			} else {
				Inventory.Transfer<T> (AcceptorInventory, 1);
			}
		}
	}
}