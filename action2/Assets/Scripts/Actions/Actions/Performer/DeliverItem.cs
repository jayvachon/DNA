using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : PerformerAction where T : ItemHolder {

		public override string Name {
			get { return "Deliver " + (typeof (T)); }
		}

		public DeliverItem (float duration) : base (duration) {}
		
	}
}