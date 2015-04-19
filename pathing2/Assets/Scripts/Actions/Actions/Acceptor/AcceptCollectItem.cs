using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectItem<T> : AcceptInventoryAction<T> where T : ItemHolder {
		
		public override bool Enabled {
			get { return Holder.Count > 0 && ConditionMet; }
		}

		public AcceptCollectItem (AcceptCondition acceptCondition=null) : base (acceptCondition) {}
	}
}