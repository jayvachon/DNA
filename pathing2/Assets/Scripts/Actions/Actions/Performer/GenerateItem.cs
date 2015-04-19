using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItem<T> : InventoryAction<T> where T : ItemHolder {

		public GenerateItem (float duration, PerformCondition performCondition=null) : base (duration, true, true, performCondition) {}
		
		public override void OnEnd () {
			Holder.Add ();
		}
	}
}