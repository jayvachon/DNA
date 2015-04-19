using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Deliver " + typeName;
				}
				return name;
			}
		}

		public override System.Type RequiredPair {
			get { return typeof (CollectItem<T>); }
		}

		public override bool CanPerform {
			get { return Holder.Count > 0; }
		}

		public IActionAcceptor Acceptor { get; set; }

		public DeliverItem (float duration) : base (duration) {}
	
		public override void OnEnd () {
			AcceptorInventory.Transfer<T> (Inventory, 1, AcceptCondition.Transferable);
		}		
	}
}