using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {
	
	public class ElderCondition : AcceptCondition {

		public override bool Acceptable {
			get {
				if (requestSick) {
					return Holder.Has (IsSick);
				} else {
					return Holder.Has (IsHealthy);
				}
			}
		}

		public override ItemHasAttribute Transferable {
			get {
				if (requestSick) {
					return IsSick;
				} else {
					return IsHealthy;
				}
			}
		}

		ElderHolder Holder {
			get {
				if (checkMyInventory) {
					return Inventory.Get<ElderHolder> () as ElderHolder;
				} else {
					return PerformerInventory.Get<ElderHolder> () as ElderHolder;
				}
			}
		}


		bool IsSick (Item item) {
			ElderItem elder = item as ElderItem;
			return elder.HealthManager.Sick;
		}

		bool IsHealthy (Item item) {
			ElderItem elder = item as ElderItem;
			return !elder.HealthManager.Sick;
		}

		bool requestSick;
		bool checkMyInventory;

		public ElderCondition (bool requestSick, bool checkMyInventory) {
			this.requestSick = requestSick;
			this.checkMyInventory = checkMyInventory;
		}
	}
}