using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {
	
	public class DefaultCondition : AcceptCondition {
		
		public override bool CanAccept {
			get { return true; }
		}
		
		public override ItemHasAttribute Transferable {
			get { return TransferableDefault; }
		}

		bool TransferableDefault (Item item) {
			return true;
		}
	}
}