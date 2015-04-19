using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptDeliverItem<T> : AcceptInventoryAction<T> where T : ItemHolder {
		
		public override bool Enabled {
			get { return !Holder.Full && ConditionMet; }
		}

		public AcceptDeliverItem (AcceptCondition acceptCondition=null) : base (acceptCondition) {}
	}
}