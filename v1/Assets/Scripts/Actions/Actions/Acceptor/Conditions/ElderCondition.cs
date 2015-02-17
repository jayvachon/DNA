using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {
	
	public class ElderCondition : AcceptCondition {

		public override bool Acceptable {
			get {
				ElderHolder holder;
				if (checkMyInventory){
					holder = Inventory.Get<ElderHolder> () as ElderHolder;
				} else {
					holder = PerformerInventory.Get<ElderHolder> () as ElderHolder;
				}
				
				if (requestSick) {
					return holder.HasSick ();
				} else {
					return holder.HasHealthy ();
				}
			}
		}

		// CollectItem is referencing this (but should not)
		public readonly bool requestSick;
		bool checkMyInventory;

		public ElderCondition (bool requestSick, bool checkMyInventory) {
			// TODO: make this an enum instead
			this.requestSick = requestSick;
			this.checkMyInventory = checkMyInventory;
		}
	}
}